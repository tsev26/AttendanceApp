using Attendance.Domain.Models;
using Attendance.WPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml.XPath;
using static Attendance.WPF.Functions.TimeFunction;

namespace Attendance.WPF.Stores
{
    public class CurrentUser
    {
        private List<AttendanceRecord> _attendanceRecords = new List<AttendanceRecord>();
        private List<AttendanceTotal> _attendanceTotal = new List<AttendanceTotal>();

        public event Action CurrentUserChange;
        public event Action CurrentUserKeysChange;
        //public event Action CurrentUserUpdatesChange;

        private User? _user;
        public User? User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
                CurrentUserChange?.Invoke();
            }
        }

        public AttendanceRecord? CurrentAttendanceRecord => AttendanceRecords.LastOrDefault(a => a.Entry <= DateTime.Now);

        /*
        private User? _userUpdates;
        public User? UserUpdates
        {
            get
            {
                return _userUpdates;
            }
            set
            {
                _userUpdates = value;
                CurrentUserUpdatesChange?.Invoke();
            }
        }
        */

        public Key SelectedKeyValue { get; set; }

        public void LoadUser(User user)
        {
            User = user;
            //_userAttendanceRecords = _attendanceRecords.Where(a => a.User == user).ToList();
            //_userAttendanceTotal = _attendanceTotal.Where(a => a.User == user).ToList();
        }

        public List<AttendanceRecord> AttendanceRecords => _attendanceRecords.Where(a => a.User == User).ToList();

        public List<AttendanceTotal> AttendanceTotals => _attendanceTotal.Where(a => a.User == User).ToList();

        public void SetActivity(Activity activity)
        {
            AttendanceRecord attendanceRecord = new AttendanceRecord(User, activity, DateTime.Now);
            _attendanceRecords.Add(attendanceRecord);
            CountTotalDay(attendanceRecord);
        }

        public string MonthAverage()
        {
            long CountDaysWorked = AttendanceTotals.Where(a => a.Date.Month == DateTime.Now.Month && a.Date.Year == DateTime.Now.Year && a.Activity.Property.Count && !a.Activity.Property.IsPause && a.Duration > TimeSpan.Zero).Count();
            long MonthlyWorked = (long)AttendanceTotals.Where(a => a.Date.Month == DateTime.Now.Month && a.Date.Year == DateTime.Now.Year && a.Activity.Property.Count && !a.Activity.Property.IsPause).Sum(a => a.Duration.TotalSeconds);
            return ConvertSecondsToHHMMSS(MonthlyWorked/CountDaysWorked);
        }

        public string WorkedInDayTotal(DateOnly date)
        {
            long WorkedInDay = (long)AttendanceTotals.Where(a => a.Date == date && a.Activity.Property.Count).Sum(a => a.Duration.TotalSeconds);

            if (AttendanceRecords.Count > 0)
            {
                AttendanceRecord lastRecord = AttendanceRecords.Last();
                if (DateOnly.FromDateTime(lastRecord.Entry) == date && lastRecord.Activity.Property.Count)
                {
                    WorkedInDay += (long)(DateTime.Now - lastRecord.Entry).TotalSeconds;
                }
            }

            return ConvertSecondsToHHMMSS(WorkedInDay);
        }

        public string WorkedInDay(DateOnly date)
        {
            long WorkedInDay = (long)AttendanceTotals.Where(a => a.Date == date && a.Activity.Property.Count && !a.Activity.Property.IsPause).Sum(a => a.Duration.TotalSeconds);

            if (AttendanceRecords.Count > 0)
            {
                AttendanceRecord lastRecord = AttendanceRecords.Last();
                if (DateOnly.FromDateTime(lastRecord.Entry) == date && lastRecord.Activity.Property.Count && !lastRecord.Activity.Property.IsPause)
                {
                    WorkedInDay += (long)(DateTime.Now - lastRecord.Entry).TotalSeconds;
                }
            }

            return ConvertSecondsToHHMMSS(WorkedInDay);
        }

        public string PauseInDay(DateOnly date)
        {
            long WorkedInDay = (long)AttendanceTotals.Where(a => a.Date == date && a.Activity.Property.Count && a.Activity.Property.IsPause).Sum(a => a.Duration.TotalSeconds);

            if (AttendanceRecords.Count > 0)
            {
                AttendanceRecord lastRecord = AttendanceRecords.Last();
                if (DateOnly.FromDateTime(lastRecord.Entry) == date && lastRecord.Activity.Property.Count && lastRecord.Activity.Property.IsPause)
                {
                    WorkedInDay += (long)(DateTime.Now - lastRecord.Entry).TotalSeconds;
                }
            }

            return ConvertSecondsToHHMMSS(WorkedInDay);
        }

        public List<AttendanceTotal> ActivitiesTotalInDay(DateOnly date)
        {
            List<AttendanceTotal> ActivitiesTotalInDay = new List<AttendanceTotal>();
            ActivitiesTotalInDay = AttendanceTotals.Where(a => a.Date == date && a.Activity.Property.Count).OrderBy(a => a.Duration).ToList().ConvertAll(x => new AttendanceTotal(x.User, x.Date, x.Activity, x.Duration));
            if (AttendanceRecords.Count > 0)
            {
                AttendanceRecord lastRecord = AttendanceRecords.Last();
                if (DateOnly.FromDateTime(lastRecord.Entry) == date && lastRecord.Activity.Property.Count)
                {
                    AttendanceTotal? AttendanceWithOngoingActivity = ActivitiesTotalInDay.SingleOrDefault(a => a.Activity == lastRecord.Activity);
                    if (AttendanceWithOngoingActivity != null)
                    {                        
                        AttendanceWithOngoingActivity.Duration += (DateTime.Now - lastRecord.Entry);
                    }
                    else
                    {
                        ActivitiesTotalInDay.Add(new AttendanceTotal(lastRecord.User, date, lastRecord.Activity, (DateTime.Now - lastRecord.Entry)));
                    }                  
                }
            }
            return ActivitiesTotalInDay.OrderByDescending(a => a.Duration).ToList();
        }

        public List<AttendanceRecord> RecordsInDay(DateOnly date)
        {
            List<AttendanceRecord> RecordsInDay = AttendanceRecords.Where(a => DateOnly.FromDateTime(a.Entry) == date).OrderByDescending(a => a.Entry).ToList();
            return RecordsInDay;
        }

        public void CountTotalDay(AttendanceRecord attendanceRecord)
        {
            DateOnly date = DateOnly.FromDateTime(attendanceRecord.Entry);
            
            List<AttendanceRecordWithEnding> attendanceRecordWithEndings = new List<AttendanceRecordWithEnding>();
            List<AttendanceRecord> orderedAttendanceRecord = AttendanceRecords.OrderBy(a => a.Entry).ToList();

            for (int i = 0; i < orderedAttendanceRecord.Count - 1; i++)
            {
                if (DateOnly.FromDateTime(orderedAttendanceRecord[i].Entry) == date && DateOnly.FromDateTime(orderedAttendanceRecord[i+1].Entry) == date)
                {
                    attendanceRecordWithEndings.Add(new AttendanceRecordWithEnding(orderedAttendanceRecord[i].User, orderedAttendanceRecord[i].Activity, orderedAttendanceRecord[i].Entry, orderedAttendanceRecord[i + 1].Entry));
                }
                else if (DateOnly.FromDateTime(orderedAttendanceRecord[i].Entry) != date && DateOnly.FromDateTime(orderedAttendanceRecord[i + 1].Entry) == date)
                {
                    DateTime startOfTheDay = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                    attendanceRecordWithEndings.Add(new AttendanceRecordWithEnding(orderedAttendanceRecord[i].User, orderedAttendanceRecord[i].Activity, startOfTheDay, orderedAttendanceRecord[i + 1].Entry));
                }
                else if (DateOnly.FromDateTime(orderedAttendanceRecord[i].Entry) == date && DateOnly.FromDateTime(orderedAttendanceRecord[i + 1].Entry) != date)
                {
                    DateTime endOfTheDay = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                    attendanceRecordWithEndings.Add(new AttendanceRecordWithEnding(orderedAttendanceRecord[i].User, orderedAttendanceRecord[i].Activity, orderedAttendanceRecord[i].Entry, endOfTheDay));
                }

            }

            /*
            if (DateOnly.FromDateTime(orderedAttendanceRecord.Last().Entry) == date)
            {
                attendanceRecordWithEndings.Add(new AttendanceRecordWithEnding(orderedAttendanceRecord.Last().User, orderedAttendanceRecord.Last().Activity, orderedAttendanceRecord.Last().Entry, null));
            }
            */

            List<AttendanceTotal> newAttendanceTotalInDay = attendanceRecordWithEndings
                .GroupBy(d => new { d.User, d.Activity, date })
                .Select(g => new AttendanceTotal
                {
                    User = g.Key.User,
                    Date = g.Key.date,
                    Activity = g.Key.Activity,
                    Duration = TimeSpan.FromSeconds(g.Sum(e => (e.Exit - e.Entry).TotalSeconds))
                })
                .ToList();

            _attendanceTotal.RemoveAll(a => a.User == User);
            _attendanceTotal.AddRange(newAttendanceTotalInDay);

        }

        public void Clear()
        {
            User = null;
        }

        public void RemoveKey(Key selectedKey)
        {
            User.Keys.Remove(selectedKey);
            CurrentUserKeysChange?.Invoke();
        }

        public void UpsertKey(Key newKeyValue)
        {
            Key? existingKey = User.Keys.FirstOrDefault(a => a.Id == newKeyValue.Id);
            if (existingKey != null)
            {
                int index = User.Keys.IndexOf(existingKey);
                User.Keys[index] = newKeyValue;
            }
            else
            {
                User.Keys.Add(newKeyValue);
            }
            CurrentUserKeysChange?.Invoke();
        }
    }
}
