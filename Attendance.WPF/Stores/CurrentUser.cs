using Attendance.Domain.Models;
using Attendance.WPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml.XPath;
using static Attendance.WPF.Functions.TimeFunction;

namespace Attendance.WPF.Stores
{
    public class CurrentUser
    {
        private User _user;
        private List<AttendanceRecord> _attendanceRecords;
        private List<AttendanceTotal> _attendanceTotal;

        public CurrentUser()
        {
            _attendanceRecords = new List<AttendanceRecord>();
            _attendanceTotal = new List<AttendanceTotal>();
        }

        public User User
        {
            get { return _user; }
            set { _user = value; }
        }

        public List<AttendanceRecord> AttendanceRecords => _attendanceRecords;

        public List<AttendanceTotal> AttendanceTotals => _attendanceTotal;

        public void SetActivity(Activity activity)
        {
            AttendanceRecord attendanceRecord = new AttendanceRecord(_user, activity, DateTime.Now);
            _attendanceRecords.Add(attendanceRecord);
            CountTotalDay(attendanceRecord);
        }

        public string MonthAverage()
        {
            int CountDaysWorked = _attendanceTotal.Where(a => a.Date.Month == DateTime.Now.Month && a.Date.Year == DateTime.Now.Year && a.Activity.Property.Count && !a.Activity.Property.IsPause && a.Duration > TimeSpan.Zero).Count();
            int MonthlyWorked = _attendanceTotal.Where(a => a.Date.Month == DateTime.Now.Month && a.Date.Year == DateTime.Now.Year && a.Activity.Property.Count && !a.Activity.Property.IsPause).Sum(a => a.Duration.Seconds);
            return ConvertSecondsToHHMMSS(MonthlyWorked/CountDaysWorked);
        }

        public string WorkedInDayTotal(DateOnly date)
        {
            int WorkedInDay = _attendanceTotal.Where(a => a.Date == date && a.Activity.Property.Count).Sum(a => a.Duration.Seconds);
            return ConvertSecondsToHHMMSS(WorkedInDay);
        }

        public string WorkedInDay(DateOnly date)
        {
            int WorkedInDay = _attendanceTotal.Where(a => a.Date == date && a.Activity.Property.Count && !a.Activity.Property.IsPause).Sum(a => a.Duration.Seconds);
            return ConvertSecondsToHHMMSS(WorkedInDay);
        }

        public string PauseInDay(DateOnly date)
        {
            int WorkedInDay = _attendanceTotal.Where(a => a.Date == date && a.Activity.Property.Count && a.Activity.Property.IsPause).Sum(a => a.Duration.Seconds);
            return ConvertSecondsToHHMMSS(WorkedInDay);
        }

        public List<AttendanceTotal> ActivitiesTotalInDay(DateOnly date)
        {
            List<AttendanceTotal> ActivitiesTotalInDay = _attendanceTotal.Where(a => a.Date == date && a.Activity.Property.Count).OrderBy(a => a.Duration).ToList();
            //if (_attendanceTotal.)
            return ActivitiesTotalInDay;
        }

        public List<AttendanceRecord> RecordsInDay(DateOnly date)
        {
            List<AttendanceRecord> RecordsInDay = _attendanceRecords.Where(a => DateOnly.FromDateTime(a.Entry) == date && a.Activity.Property.Count).OrderByDescending(a => a.Entry).ToList();
            return RecordsInDay;
        }

        public void CountTotalDay(AttendanceRecord attendanceRecord)
        {
            DateOnly date = DateOnly.FromDateTime(attendanceRecord.Entry);
            
            List<AttendanceRecordWithEnding> attendanceRecordWithEndings = new List<AttendanceRecordWithEnding>();
            List<AttendanceRecord> orderedAttendanceRecord = _attendanceRecords.OrderByDescending(a => a.Entry).ToList();

            for (int i = 0; i < orderedAttendanceRecord.Count - 1; i++)
            {
                if (DateOnly.FromDateTime(orderedAttendanceRecord[i].Entry) == date && DateOnly.FromDateTime(orderedAttendanceRecord[i+1].Entry) == date)
                {
                    attendanceRecordWithEndings.Add(new AttendanceRecordWithEnding(orderedAttendanceRecord[i].User, orderedAttendanceRecord[i].Activity, orderedAttendanceRecord[i + 1].Entry, orderedAttendanceRecord[i].Entry));
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

            _attendanceTotal.Clear();
            _attendanceTotal.AddRange(newAttendanceTotalInDay);

        }
    }
}
