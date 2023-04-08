using Attendance.Domain.Models;
using Attendance.WPF.Model;
using Attendance.WPF.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Attendance.WPF.Functions.TimeFunction;
using Activity = Attendance.Domain.Models.Activity;

namespace Attendance.WPF.Stores
{
    public class AttendanceRecordStore
    {
        private List<AttendanceRecord> _attendanceRecords = new List<AttendanceRecord>();
        private List<AttendanceTotal> _attendanceTotal = new List<AttendanceTotal>();
        private List<AttendanceRecordFix> _attendanceRecordFixes = new List<AttendanceRecordFix>();

        public event Action CurrentAttendanceChange;
        public event Action CurrentAttendanceRecordFixChange;

		public List<AttendanceRecord> AttendanceRecords(User user) 
        {
            return _attendanceRecords.Where(a => a.User == user).ToList();
        }

        public List<AttendanceTotal> AttendanceTotals(User user)
        {
            return _attendanceTotal.Where(a => a.User == user).ToList();
        }

        public List<AttendanceRecordFix> AttendanceRecordFixes(User user)
        {
            return _attendanceRecordFixes.Where(a => a.User == user).ToList();
        }

        public AttendanceRecord? CurrentAttendanceRecord(User user)
        {
            AttendanceRecord lastAttendanceRecord = AttendanceRecords(user).Where(a => a.Entry <= DateTime.Now).OrderBy(a => a.Entry).LastOrDefault();
            return lastAttendanceRecord;
        }


        public void AddAttendanceRecord(User user, Activity activity)
        {
            AttendanceRecord attendanceRecord = new AttendanceRecord(user, activity, DateTime.Now);
            _attendanceRecords.Add(attendanceRecord);
            CountTotalDay(user, attendanceRecord);
            CurrentAttendanceChange?.Invoke();
        }
        public void AddAttendanceRecord(User user, Activity activity, DateTime dateTime)
        {
            AttendanceRecord attendanceRecord = new AttendanceRecord(user, activity, dateTime);
            _attendanceRecords.Add(attendanceRecord);
            CountTotalDay(user, attendanceRecord);
            CurrentAttendanceChange?.Invoke();
        }


        public void AddAttendanceRecord(User user, Activity activity, DateTime dateTime, AttendanceRecordDetail attendanceRecordDetail)
        {
            AttendanceRecord attendanceRecord = new AttendanceRecord(user, activity, dateTime, attendanceRecordDetail);
            _attendanceRecords.Add(attendanceRecord);
            CountTotalDay(user, attendanceRecord);
            CurrentAttendanceChange?.Invoke();
        }

        public void UpdateAttendanceRecord(User user, Activity activity, DateTime dateTime, AttendanceRecord oldAttendanceRecord)
        {
            AttendanceRecord newAttendanceRecord = new AttendanceRecord(user, activity, dateTime);

            int index = _attendanceRecords.FindIndex(a => a.Id == oldAttendanceRecord.Id);
            if (index != -1)
            {
                _attendanceRecords[index] = newAttendanceRecord;
            }
            CountTotalDay(user, newAttendanceRecord, oldAttendanceRecord);
            CurrentAttendanceChange?.Invoke();
        }


        public void AddAttendanceRecordFixDelete(User user, AttendanceRecord attendanceRecord)
        {
            AttendanceRecordFix attendanceRecordFix = new AttendanceRecordFix(attendanceRecord, user);
            _attendanceRecordFixes.Add(attendanceRecordFix);
            CurrentAttendanceRecordFixChange?.Invoke();
        }

        public void AddAttendanceRecordFixUpdate(User user, AttendanceRecord attendanceRecord, Activity activity, DateTime entry)
        {
            AttendanceRecordFix attendanceRecordFix = new AttendanceRecordFix(attendanceRecord, user, activity, entry);
            _attendanceRecordFixes.Add(attendanceRecordFix);
            CurrentAttendanceRecordFixChange?.Invoke();
        }

        public void AddAttendanceRecordFixInsert(User user, Activity activity, DateTime entry)
        {
            AttendanceRecordFix attendanceRecordFix = new AttendanceRecordFix(user, activity, entry);
            _attendanceRecordFixes.Add(attendanceRecordFix);
            CurrentAttendanceRecordFixChange?.Invoke();
        }

        public void RemoveAttendanceRecordFix(AttendanceRecordFix attendanceRecordFix)
        {
            _attendanceRecordFixes.Remove(attendanceRecordFix);
            CurrentAttendanceRecordFixChange?.Invoke();
        }

        public void RemoveAttendanceRecord(AttendanceRecord attendanceRecord)
        {
            _attendanceRecords.Remove(attendanceRecord);
            if (attendanceRecord.AttendanceRecordDetail != null)
            {
                AttendanceRecord endOfPlan = _attendanceRecords.FirstOrDefault(a => a.User == attendanceRecord.User && a.Entry == attendanceRecord.AttendanceRecordDetail.ExpectedEnd);
                _attendanceRecords.Remove(endOfPlan);
            }
            CountTotalDay(attendanceRecord.User, attendanceRecord);
            CurrentAttendanceChange?.Invoke();
        }

        public string WorkedInDayTotal(User user, DateOnly date)
        {
            long WorkedInDay = CountDuration(user, date, true);

            return ConvertSecondsToHHMMSS(WorkedInDay);
        }

        public string WorkedInDay(User user, DateOnly date)
        {
            long WorkedInDay = CountDuration(user ,date, false, true);

            return ConvertSecondsToHHMMSS(WorkedInDay);
        }

        public string PauseInDay(User user, DateOnly date)
        {
            long WorkedInDay = CountDuration(user, date, false, false);

            return ConvertSecondsToHHMMSS(WorkedInDay);
        }

        public void CountTotalDay(User user, AttendanceRecord attendanceRecord, AttendanceRecord? oldAttendanceRecord = null)
        {

            DateOnly date = DateOnly.FromDateTime(attendanceRecord.Entry);

            List<AttendanceRecordWithEnding> attendanceRecordWithEndings = new List<AttendanceRecordWithEnding>();
            List<AttendanceRecord> orderedAttendanceRecord = AttendanceRecords(user).OrderBy(a => a.Entry).ToList();


            for (int i = 0; i < orderedAttendanceRecord.Count - 1; i++)
            {
                DateTime startDateTime = orderedAttendanceRecord[i].Entry;
                DateTime endDateTime = orderedAttendanceRecord[i + 1].Entry;
                DateOnly startDate = DateOnly.FromDateTime(startDateTime);
                DateOnly endDate = DateOnly.FromDateTime(endDateTime);

                if (startDate == endDate)
                {
                    attendanceRecordWithEndings.Add(new AttendanceRecordWithEnding(orderedAttendanceRecord[i].User, orderedAttendanceRecord[i].Activity, startDateTime, endDateTime));
                }
                else
                {
                    for (DateOnly dateS = startDate; dateS <= endDate; dateS = dateS.AddDays(1))
                    {
                        if (dateS == startDate)
                        {
                            DateTime endOfTheDay = new DateTime(dateS.Year, dateS.Month, dateS.Day, 23, 59, 59);
                            attendanceRecordWithEndings.Add(new AttendanceRecordWithEnding(orderedAttendanceRecord[i].User, orderedAttendanceRecord[i].Activity, startDateTime, endOfTheDay));
                        }
                        else if (dateS == endDate)
                        {
                            DateTime startOfTheDay = new DateTime(dateS.Year, dateS.Month, dateS.Day, 0, 0, 0);
                            attendanceRecordWithEndings.Add(new AttendanceRecordWithEnding(orderedAttendanceRecord[i].User, orderedAttendanceRecord[i].Activity, startOfTheDay, endDateTime));
                        }
                        else
                        {
                            DateTime startOfTheDay = new DateTime(dateS.Year, dateS.Month, dateS.Day, 0, 0, 0);
                            DateTime endOfTheDay = new DateTime(dateS.Year, dateS.Month, dateS.Day, 23, 59, 59);
                            attendanceRecordWithEndings.Add(new AttendanceRecordWithEnding(orderedAttendanceRecord[i].User, orderedAttendanceRecord[i].Activity, startOfTheDay, endOfTheDay));
                        }
                    }
                }
            }

            List<AttendanceTotal> newAttendanceTotalInDay = attendanceRecordWithEndings.Where(a => a.Activity.Property.Count)
                .GroupBy(d => new { d.User, d.Activity, d.Entry.Date })
                .Select(g => new AttendanceTotal
                {
                    User = g.Key.User,
                    Date = DateOnly.FromDateTime(g.Key.Date),
                    Activity = g.Key.Activity,
                    Duration = TimeSpan.FromSeconds(g.Sum(e => (e.Exit - e.Entry).TotalSeconds))
                })
                .ToList();

            _attendanceTotal.RemoveAll(a => a.User == user);
            _attendanceTotal.AddRange(newAttendanceTotalInDay);

        }

        private long CountDuration(User user, DateOnly date, bool all, bool work = true)
        {
            List<AttendanceTotal> attendanceTotals = AttendanceTotals(user).Where(a => a.Date == date && a.Activity.Property.Count && (a.Activity.Property.IsWork == work || all)).ToList();
            long CountInDay = (long)attendanceTotals.Sum(a => a.Duration.TotalSeconds);

            List<AttendanceRecord> attendanceRecord = AttendanceRecords(user).Where(a => a.Entry <= DateTime.Now).ToList();
            if (attendanceRecord.Count > 0)
            {
                AttendanceRecord lastRecord = attendanceRecord.OrderBy(a => a.Entry).Last();
                if (lastRecord.Activity.Property.Count && (lastRecord.Activity.Property.IsWork == work || all) && DateOnly.FromDateTime(lastRecord.Entry) <= date)
                {
                    DateOnly dateS = DateOnly.FromDateTime(lastRecord.Entry);
                    DateOnly now = DateOnly.FromDateTime(DateTime.Now);
                    DateTime startOfTheDay = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                    DateTime endOfTheDay = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                    if (dateS == date && now == date)
                    {
                        CountInDay += (long)(DateTime.Now - lastRecord.Entry).TotalSeconds;
                    }
                    else if (dateS != date && now == date)
                    {
                        CountInDay += (long)(DateTime.Now - startOfTheDay).TotalSeconds;
                    }
                    else if (dateS == date && now != date)
                    {
                        CountInDay += (long)(endOfTheDay - lastRecord.Entry).TotalSeconds;
                    }
                    else if (dateS != date && now != date)
                    {
                        CountInDay += (long)(endOfTheDay - startOfTheDay).TotalSeconds;
                    }
                }
            }

            return CountInDay;
        }

        public List<AttendanceRecordItem> RecordsInDay(User user, DateOnly date)
        {
            //List<AttendanceRecord> RecordsInDay = AttendanceRecords.Where(a => DateOnly.FromDateTime(a.Entry) == date).ToList();
            List<AttendanceRecordItem> recordsInDay = AttendanceRecords(user).Where(a => DateOnly.FromDateTime(a.Entry) == date).OrderByDescending(a => a.Entry).Select(a => new AttendanceRecordItem(a, a.Activity, a.Entry.ToString("HH:mm"))).ToList();

            AttendanceRecord oldRecord = AttendanceRecords(user).Where(a => DateOnly.FromDateTime(a.Entry) < date).OrderBy(a => a.Entry).LastOrDefault();
            if (oldRecord != null && oldRecord.Activity.Property.Count)
            {
                //DateTime startOfTheDay = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                //RecordsInDay.Add(new AttendanceRecord(oldRecord.User, oldRecord.Activity, startOfTheDay));
                recordsInDay.Add(new AttendanceRecordItem(oldRecord, oldRecord.Activity, oldRecord.Entry.ToString("dd.MM HH:mm")));
            }

            return recordsInDay.ToList();
        }

        public List<MonthlyAttendanceTotalsWork> MonthlyAttendanceTotalsWorks(User user, int month, int year)
        {

            IEnumerable<DateOnly> dates = Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                .Select(day => new DateOnly(year, month, day))
                .Where(date => date < DateOnly.FromDateTime(DateTime.Now)); ;

            List<MonthlyAttendanceTotalsWork> result = new List<MonthlyAttendanceTotalsWork>();
            foreach (DateOnly date in dates)
            {
                result.Add(new MonthlyAttendanceTotalsWork()
                {
                    Date = date,
                    DateName = CultureInfo.GetCultureInfo("cs-CZ").DateTimeFormat.GetDayName(date.DayOfWeek),
                    User = user,
                    Worked = TimeSpan.FromSeconds(CountDuration(user, date, false, true))
                });
            }

            return result;
        }

        public List<AttendanceTotal> ActivitiesTotalInDay(User user, DateOnly date)
        {
            List<AttendanceTotal> ActivitiesTotalInDay = new List<AttendanceTotal>();

            List<AttendanceTotal> ActivitiesTotalInDayX = AttendanceTotals(user).Where(a => a.Date == date && a.Activity.Property.Count).ToList();
            ActivitiesTotalInDay = ActivitiesTotalInDayX.ConvertAll(x => new AttendanceTotal(x.User, x.Date, x.Activity, x.Duration));


            List<AttendanceRecord> attendanceRecord = AttendanceRecords(user).Where(a => a.Entry <= DateTime.Now).ToList();
            if (attendanceRecord.Count > 0)
            {
                AttendanceRecord lastRecord = attendanceRecord.OrderBy(a => a.Entry).Last();
                if (lastRecord.Activity.Property.Count && DateOnly.FromDateTime(lastRecord.Entry) <= date)
                {
                    DateOnly dateS = DateOnly.FromDateTime(lastRecord.Entry);
                    DateOnly now = DateOnly.FromDateTime(DateTime.Now);
                    DateTime startOfTheDay = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                    DateTime endOfTheDay = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                    if (dateS == date && now == date)
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
                    else if (dateS != date && now == date)
                    {
                        AttendanceTotal? AttendanceWithOngoingActivity = ActivitiesTotalInDay.SingleOrDefault(a => a.Activity == lastRecord.Activity);
                        if (AttendanceWithOngoingActivity != null)
                        {
                            AttendanceWithOngoingActivity.Duration += (DateTime.Now - startOfTheDay);
                        }
                        else
                        {
                            ActivitiesTotalInDay.Add(new AttendanceTotal(lastRecord.User, date, lastRecord.Activity, (DateTime.Now - startOfTheDay)));
                        }
                    }
                    else if (dateS == date && now != date)
                    {
                        AttendanceTotal? AttendanceWithOngoingActivity = ActivitiesTotalInDay.SingleOrDefault(a => a.Activity == lastRecord.Activity);
                        if (AttendanceWithOngoingActivity != null)
                        {
                            AttendanceWithOngoingActivity.Duration += (endOfTheDay - lastRecord.Entry);
                        }
                        else
                        {
                            ActivitiesTotalInDay.Add(new AttendanceTotal(lastRecord.User, date, lastRecord.Activity, (endOfTheDay - lastRecord.Entry)));
                        }
                    }
                    else if (dateS != date && now != date)
                    {
                        AttendanceTotal? AttendanceWithOngoingActivity = ActivitiesTotalInDay.SingleOrDefault(a => a.Activity == lastRecord.Activity);
                        if (AttendanceWithOngoingActivity != null)
                        {
                            AttendanceWithOngoingActivity.Duration += (endOfTheDay - startOfTheDay);
                        }
                        else
                        {
                            ActivitiesTotalInDay.Add(new AttendanceTotal(lastRecord.User, date, lastRecord.Activity, (endOfTheDay - startOfTheDay)));
                        }
                    }
                }
            }

            return ActivitiesTotalInDay.OrderByDescending(a => a.Duration).ToList();
        }

        public List<AttendanceRecordFix> GetPendingFixes(User user)
        {
            List<AttendanceRecordFix> records = new List<AttendanceRecordFix>();

            records.AddRange(_attendanceRecordFixes.Where(a => a.Approved == ApproveType.Waiting && (a.User.IsSubordinate(user))));

            return records;
        }

        public void ApproveFix(AttendanceRecordFix attendanceRecordFix)
        {
            switch (attendanceRecordFix.FixType)
            {
                case FixType.Insert:
                    AddAttendanceRecord(attendanceRecordFix.User, attendanceRecordFix.Activity, attendanceRecordFix.Entry);
                    break;
                case FixType.Update:
                    UpdateAttendanceRecord(attendanceRecordFix.User, attendanceRecordFix.Activity, attendanceRecordFix.Entry, attendanceRecordFix.AttendanceRecord);
                    break;
                case FixType.Delete:
                    RemoveAttendanceRecord(attendanceRecordFix.AttendanceRecord);
                    break;
            }
            attendanceRecordFix.Approved = ApproveType.Approved;
            CurrentAttendanceRecordFixChange?.Invoke();
        }

        public void RejectFix(AttendanceRecordFix attendanceRecordFix)
        {
            attendanceRecordFix.Approved = ApproveType.Rejected;
            CurrentAttendanceRecordFixChange?.Invoke();
        }
    }
}
