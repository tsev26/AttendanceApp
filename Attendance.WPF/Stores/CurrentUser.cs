﻿using Attendance.Domain.Models;
using Attendance.WPF.Model;
using Attendance.WPF.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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

        public void SetActivity(Activity activity, DateTime dateTime)
        {
            AttendanceRecord attendanceRecord = new AttendanceRecord(User, activity, dateTime);
            _attendanceRecords.Add(attendanceRecord);
            CountTotalDay(attendanceRecord);
        }

        public void SetActivity(Activity activity, DateTime dateTime, AttendanceRecordDetail attendanceRecordDetail)
        {
            AttendanceRecord attendanceRecord = new AttendanceRecord(User, activity, dateTime, attendanceRecordDetail);
            _attendanceRecords.Add(attendanceRecord);
            CountTotalDay(attendanceRecord);
        }

        /*
        public string MonthAverage()
        {
            long CountDaysWorked = AttendanceTotals.Where(a => a.Date.Month == DateTime.Now.Month && a.Date.Year == DateTime.Now.Year && a.Activity.Property.Count && !a.Activity.Property.IsPause && a.Duration > TimeSpan.Zero).Count();
            long MonthlyWorked = (long)AttendanceTotals.Where(a => a.Date.Month == DateTime.Now.Month && a.Date.Year == DateTime.Now.Year && a.Activity.Property.Count && !a.Activity.Property.IsPause).Sum(a => a.Duration.TotalSeconds);
            return ConvertSecondsToHHMMSS(MonthlyWorked/CountDaysWorked);
        }
        */

        public string WorkedInDayTotal(DateOnly date)
        {
            long WorkedInDay = CountDuration(date, true);

            return ConvertSecondsToHHMMSS(WorkedInDay);
        }

        public string WorkedInDay(DateOnly date)
        {
            long WorkedInDay = CountDuration(date, false, true);

            return ConvertSecondsToHHMMSS(WorkedInDay);
        }

        public string PauseInDay(DateOnly date)
        {
            long WorkedInDay = CountDuration(date, false, false);

            return ConvertSecondsToHHMMSS(WorkedInDay);
        }

        private long CountDuration(DateOnly date, bool all, bool work = true)
        {
            List<AttendanceTotal> attendanceTotals = AttendanceTotals.Where(a => a.Date == date && a.Activity.Property.Count && (a.Activity.Property.IsWork == work || all)).ToList();
            long CountInDay = (long)attendanceTotals.Sum(a => a.Duration.TotalSeconds);

            List<AttendanceRecord> attendanceRecord = AttendanceRecords.Where(a => a.Entry <= DateTime.Now).ToList();
            if (attendanceRecord.Count > 0)
            {
                AttendanceRecord lastRecord = attendanceRecord.Last();
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

        private long CountDuration(DateOnly date)
        {
            long CountInDay = 0;

            return CountInDay;
        }

        public List<AttendanceTotal> ActivitiesTotalInDay(DateOnly date)
        {
            List<AttendanceTotal> ActivitiesTotalInDay = new List<AttendanceTotal>();

            List<AttendanceTotal> ActivitiesTotalInDayX = AttendanceTotals.Where(a => a.Date == date && a.Activity.Property.Count).ToList();
            ActivitiesTotalInDay = ActivitiesTotalInDayX.ConvertAll(x => new AttendanceTotal(x.User, x.Date, x.Activity, x.Duration));


            List<AttendanceRecord> attendanceRecord = AttendanceRecords.Where(a => a.Entry <= DateTime.Now).ToList();
            if (attendanceRecord.Count > 0)
            {
                AttendanceRecord lastRecord = attendanceRecord.Last();
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

        public List<AttendanceRecord> RecordsInDay(DateOnly date)
        {
            List<AttendanceRecord> RecordsInDay = AttendanceRecords.Where(a => DateOnly.FromDateTime(a.Entry) == date).ToList();

            AttendanceRecord oldRecord = AttendanceRecords.Where(a => DateOnly.FromDateTime(a.Entry) < date).OrderBy(a => a.Entry).LastOrDefault();
            if (oldRecord != null && oldRecord.Activity.Property.Count)
            {
                DateTime startOfTheDay = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                RecordsInDay.Add(new AttendanceRecord(oldRecord.User, oldRecord.Activity, startOfTheDay));
            }

            return RecordsInDay.OrderByDescending(a => a.Entry).ToList();
        }

        public void CountTotalDay(AttendanceRecord attendanceRecord)
        {
            DateOnly date = DateOnly.FromDateTime(attendanceRecord.Entry);
            
            List<AttendanceRecordWithEnding> attendanceRecordWithEndings = new List<AttendanceRecordWithEnding>();
            List<AttendanceRecord> orderedAttendanceRecord = AttendanceRecords.OrderBy(a => a.Entry).ToList();


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

        public List<MonthlyAttendanceTotalsWork> MonthlyAttendanceTotalsWorks(int month, int year, User user)
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
                    Worked = TimeSpan.FromSeconds(CountDuration(date, false, true))
                });
            }

            return result;
        }
    }
}
