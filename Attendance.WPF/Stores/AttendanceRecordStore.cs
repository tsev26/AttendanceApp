using Attendance.Domain.Models;
using Attendance.Domain.Models.Virtual;
using Attendance.EF.Services;
using Attendance.WPF.Controls;
using Attendance.WPF.Model;
using Attendance.WPF.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static Attendance.WPF.Functions.TimeFunction;
using Activity = Attendance.Domain.Models.Activity;

namespace Attendance.WPF.Stores
{
    public class AttendanceRecordStore
    {
        private readonly ActivityStore _activityStore;
        private readonly UserDataService _userDataService;

        public AttendanceRecordStore(ActivityStore activityStore, UserDataService userDataService)
        {
            _activityStore = activityStore;
            _userDataService = userDataService;
        }

        public List<AttendanceRecord> AttendanceRecords = new List<AttendanceRecord>();
        public List<AttendanceTotal> AttendanceTotal = new List<AttendanceTotal>();
        public List<AttendanceRecordFix> AttendanceRecordFixes = new List<AttendanceRecordFix>();
        public List<UsersCurrentActivity> UsersCurrentActivities = new List<UsersCurrentActivity>();


        public event Action CurrentAttendanceChange;
        public event Action CurrentAttendanceRecordFixChange;
        public event Action AttendanceLoad;
        public event Action UsersCurrentActivitiesLoad;

        public async Task LoadUsersCurrentActivities()
        {
            UsersCurrentActivities = await _userDataService.GetUsersCurrentActivities();
            UsersCurrentActivitiesLoad?.Invoke();
        }


        public async Task LoadAttendanceRecords(User user) 
        {
            AttendanceRecords = await _userDataService.GetAttendanceRecords(user);
            AttendanceLoad?.Invoke();
        }

        public async Task LoadAttendanceTotals(User user)
        {
            AttendanceTotal = await _userDataService.GetAttendanceTotals(user);
            AttendanceLoad?.Invoke();
        }

        public async Task LoadAttendanceRecordFixes(User user)
        {
            AttendanceRecordFixes = await _userDataService.GetAttendanceRecordFixes(user);
        }

        public AttendanceRecord? CurrentAttendanceRecord => AttendanceRecords.Where(a => a.Entry <= DateTime.Now).OrderBy(a => a.Entry).LastOrDefault();


        public async Task AddAttendanceRecord(User user, Activity activity)
        {
            await AddAttendanceRecord(user, activity, DateTime.Now);
        }
        public async Task AddAttendanceRecord(User user, Activity activity, DateTime dateTime)
        {
            AttendanceRecord attendanceRecord = new AttendanceRecord(user, activity, dateTime);
            await _userDataService.AddAttendanceRecord(attendanceRecord);
            AttendanceRecords.Add(attendanceRecord);
            //await CountTotalDayAsync(user, attendanceRecord);
            await CountTotalDay(user, attendanceRecord.Entry);
            CurrentAttendanceChange?.Invoke();
        }

        public async Task AddAttendanceRecord(User user, Activity activity, DateTime dateTime, AttendanceRecordDetail attendanceRecordDetail)
        {
            AttendanceRecord attendanceRecord = new AttendanceRecord(user, activity, dateTime, attendanceRecordDetail);
            AttendanceRecord attendanceRecordEnding = new AttendanceRecord(user, _activityStore.GlobalSetting.MainNonWorkActivity, attendanceRecordDetail.ExpectedEnd);
            await _userDataService.AddAttendanceRecord(attendanceRecord);
            AttendanceRecords.Add(attendanceRecord);
            await _userDataService.AddAttendanceRecord(attendanceRecordEnding);
            AttendanceRecords.Add(attendanceRecordEnding);
            //await CountTotalDayAsync(user, attendanceRecord);
            await CountTotalDay(user, attendanceRecord.Entry);
            CurrentAttendanceChange?.Invoke();
        }

        public async Task UpdateAttendanceRecord(User user, Activity activity, DateTime dateTime, AttendanceRecord oldAttendanceRecord)
        {
            AttendanceRecord newAttendanceRecord = new AttendanceRecord(user, activity, dateTime);

            int index = AttendanceRecords.FindIndex(a => a.ID == oldAttendanceRecord.ID);
            if (index != -1)
            {
                AttendanceRecords[index] = newAttendanceRecord;
            }

            await _userDataService.UpdateAttendanceRecord(newAttendanceRecord, oldAttendanceRecord);

            //CountTotalDayAsync(user, newAttendanceRecord, oldAttendanceRecord);

            await CountTotalDay(user, newAttendanceRecord.Entry);
            if (newAttendanceRecord.Entry.Date != oldAttendanceRecord.Entry.Date)
            {
                await CountTotalDay(user, oldAttendanceRecord.Entry);
            }

            CurrentAttendanceChange?.Invoke();
        }

        public async Task AddAttendanceRecordFixDelete(User user, AttendanceRecord attendanceRecord)
        {
            AttendanceRecordFix attendanceRecordFix = new AttendanceRecordFix(attendanceRecord, user);
            AttendanceRecordFixes.Add(attendanceRecordFix);
            await _userDataService.AddAttendanceRecordFix(attendanceRecordFix);
            CurrentAttendanceRecordFixChange?.Invoke();
        }

        public async Task AddAttendanceRecordFixUpdate(User user, AttendanceRecord attendanceRecord, Activity activity, DateTime entry)
        {
            AttendanceRecordFix attendanceRecordFix = new AttendanceRecordFix(attendanceRecord, user, activity, entry);
            AttendanceRecordFixes.Add(attendanceRecordFix);
            await _userDataService.AddAttendanceRecordFix(attendanceRecordFix);
            CurrentAttendanceRecordFixChange?.Invoke();
        }

        public async Task AddAttendanceRecordFixInsert(User user, Activity activity, DateTime entry)
        {
            AttendanceRecordFix attendanceRecordFix = new AttendanceRecordFix(user, activity, entry);
            AttendanceRecordFixes.Add(attendanceRecordFix);
            await _userDataService.AddAttendanceRecordFix(attendanceRecordFix);
            CurrentAttendanceRecordFixChange?.Invoke();
        }

        public void RemoveAttendanceRecordFix(AttendanceRecordFix attendanceRecordFix)
        {
            AttendanceRecordFixes.Remove(attendanceRecordFix);
            CurrentAttendanceRecordFixChange?.Invoke();
        }

        public async Task RemoveAttendanceRecord(AttendanceRecord attendanceRecord)
        {
            DateTime day = attendanceRecord.Entry;
            User user = attendanceRecord.User;
            AttendanceRecords.Remove(attendanceRecord);
            await _userDataService.RemoveAttendanceRecord(attendanceRecord);
            if (attendanceRecord.AttendanceRecordDetail != null)
            {
                AttendanceRecord endOfPlan = AttendanceRecords.FirstOrDefault(a => a.User == attendanceRecord.User && a.Entry == attendanceRecord.AttendanceRecordDetail.ExpectedEnd);
                AttendanceRecords.Remove(endOfPlan);
                await _userDataService.RemoveAttendanceRecord(endOfPlan);
            }
            //CountTotalDayAsync(attendanceRecord.User, attendanceRecord);
            await CountTotalDay(user, day);
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

        public async Task CountTotalDay(User user, DateTime Entry)
        {
            DateTime startDate = AttendanceRecords.OrderByDescending(a => a.Entry).Where(a => a.UserId == user.ID && a.Entry < Entry).Select(a => a.Entry.Date).FirstOrDefault();
            DateTime endDate = AttendanceRecords.OrderBy(a => a.Entry).Where(a => a.UserId == user.ID && a.Entry > Entry).Select(a => a.Entry.Date).FirstOrDefault();
            if (startDate == DateTime.MinValue) startDate = Entry.Date;
            if (endDate == DateTime.MinValue) endDate = DateTime.Now.Date;

            List<DateOnly> updatesDays = new List<DateOnly>();
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                updatesDays.Add(DateOnly.FromDateTime(date));
            }

            List<AttendanceRecord> attendanceRecords = AttendanceRecords.Where(a => a.UserId == user.ID && a.Entry.Date >= startDate.Date && a.Entry.Date <= endDate && a.Entry <= DateTime.Now).ToList();
            AttendanceRecord attendanceRecordPrevious = AttendanceRecords.OrderByDescending(a => a.Entry).Where(a => a.UserId == user.ID && a.Entry.Date < startDate.Date && a.Entry <= DateTime.Now).FirstOrDefault();
            if (attendanceRecordPrevious != null)
            {
                attendanceRecords.Add(attendanceRecordPrevious);
            }

            AttendanceRecord attendanceRecordNext = AttendanceRecords.OrderBy(a => a.Entry).Where(a => a.UserId == user.ID && a.Entry.Date > endDate.Date && a.Entry <= DateTime.Now).FirstOrDefault();
            if (attendanceRecordNext != null)
            {
                attendanceRecords.Add(attendanceRecordNext);
            }


            attendanceRecords = attendanceRecords.OrderBy(a => a.Entry).ToList();
            TimeSpan duration = TimeSpan.Zero;
            List<AttendanceTotal> attendanceActivityTotals = new List<AttendanceTotal>();
            foreach (AttendanceRecord record in attendanceRecords)
            {
                if (record.Activity.Property.Count)
                {
                    AttendanceRecord nextRecord = attendanceRecords.FirstOrDefault(a => a.Entry > record.Entry);
                    if (nextRecord == null) break;
                    DateTime startDateX = record.Entry.Date;
                    DateTime endDateX = nextRecord.Entry.Date;

                    while (startDateX <= endDateX)
                    {
                        DateTime entry = (startDateX == record.Entry.Date) ? record.Entry : startDateX;
                        DateTime exit = (startDateX == endDateX) ? nextRecord.Entry : startDateX.AddDays(1).AddSeconds(-1);
                        if (record.Activity.Property.IsPlan && !record.Activity.Property.HasTime)
                        {
                            if (record.AttendanceRecordDetail?.IsHalfDay ?? false)
                            {
                                duration = _activityStore.GlobalSetting.LenghtOfHalfDayActivity;
                            }
                            else
                            {
                                duration = _activityStore.GlobalSetting.LenghtOfAllDayActivity;
                            }
                        }
                        else
                        {
                            duration = exit - entry;
                        }
                        
                        attendanceActivityTotals.Add(new AttendanceTotal(record.User, DateOnly.FromDateTime(startDateX), record.Activity, duration));

                        startDateX = startDateX.AddDays(1);
                    }
                }
            }

            List<AttendanceTotal> attendanceTotals = new List<AttendanceTotal>();
            foreach (AttendanceTotal total in attendanceActivityTotals)
            {
                AttendanceTotal attendanceTotal = attendanceTotals.FirstOrDefault(a => a.Activity.ID == total.Activity.ID && a.Date == total.Date);
                total.Duration = TimeSpan.FromTicks((total.Duration.Ticks + TimeSpan.TicksPerSecond / 2) / TimeSpan.TicksPerSecond * TimeSpan.TicksPerSecond);
                if (attendanceTotal == null)
                {
                    attendanceTotals.Add(new AttendanceTotal(total.User, total.Date, total.Activity, total.Duration));
                }
                else
                {
                    attendanceTotal.Duration += total.Duration;
                }
            }

            //check if there is enough pause for activities
            Activity mainPauseActivity = _activityStore.GlobalSetting.MainPauseActivity;
            foreach (DateOnly dateOnly in updatesDays)
            {
                TimeSpan totalPauseInDay = TimeSpan.FromSeconds(attendanceTotals.Where(a => a.Date == dateOnly && a.Activity.Property.IsPause).Sum(a => a.Duration.TotalSeconds));
                TimeSpan totalWorkWithPauseInDay = TimeSpan.FromSeconds(attendanceTotals.Where(a => a.Date == dateOnly && !a.Activity.Property.IsPause && a.Activity.Property.HasPause).Sum(a => a.Duration.TotalSeconds));

                if (totalWorkWithPauseInDay >= _activityStore.GlobalSetting.PauseEvery)
                {
                    TimeSpan totalRequiredPause = (int)(totalWorkWithPauseInDay / _activityStore.GlobalSetting.PauseEvery) * _activityStore.GlobalSetting.PauseDuration;

                    if (totalRequiredPause > totalPauseInDay)
                    {
                        //adding required pause
                        AttendanceTotal? pause = attendanceTotals.FirstOrDefault(a => a.Date == dateOnly && a.Activity == mainPauseActivity);
                        if (pause != null)
                        {
                            pause.Duration += totalRequiredPause - totalPauseInDay;
                        }
                        else
                        {
                            AttendanceTotal pauseAttendanceTotal = new AttendanceTotal(user, dateOnly, mainPauseActivity, totalRequiredPause - totalPauseInDay);
                            pauseAttendanceTotal.User = null;
                            attendanceTotals.Add(pauseAttendanceTotal);
                        }

                        //substract required pause from worked time
                        foreach (AttendanceTotal attendanceTotal in attendanceTotals.Where(a => a.Date == dateOnly && a.Activity.Property.Count && a.Activity.Property.IsWork && a.Activity.Property.HasPause))
                        {
                            TimeSpan activityDuration = attendanceTotal.Duration;
                            if (activityDuration > totalRequiredPause)
                            {
                                activityDuration -= totalRequiredPause;
                                break;
                            }
                            else
                            {
                                activityDuration = TimeSpan.Zero;
                                totalRequiredPause -= activityDuration;
                            }
                        }
                    }
                }
            }

            //check if activity isnt reached maxed in day
            foreach (AttendanceTotal attendanceTotal in attendanceTotals)
            {
                if (attendanceTotal.Duration > attendanceTotal.Activity.Property.MaxInDay && attendanceTotal.Activity.Property.Count && attendanceTotal.Activity.Property.MaxInDay != TimeSpan.Zero)
                {
                    attendanceTotal.Duration = attendanceTotal.Activity.Property.MaxInDay;
                }
            }

            //AttendanceTotal.RemoveAll(a => a.User == user && updatesDays.Contains(a.Date));
            //AttendanceTotal.AddRange(attendanceTotals);

            foreach (var attendanceTotal in attendanceTotals)
            {
                attendanceTotal.User = null;
                attendanceTotal.Activity = null;
            }
            await _userDataService.AddAttendanceTotal(user, updatesDays, attendanceTotals);

        }

        public async Task CountTotalDayAsync(User user, AttendanceRecord attendanceRecord, AttendanceRecord? oldAttendanceRecord = null)
        {
            DateTime dateStart = attendanceRecord.Entry.Date;
            DateTime dateTimeStart = attendanceRecord.Entry;

            List<AttendanceRecordWithEnding> attendanceRecordWithEndings = new List<AttendanceRecordWithEnding>();
            List<AttendanceRecord> all = AttendanceRecords.Where(a => a.UserId == user.ID).ToList();
            AttendanceRecord nextAttendanceRecord = AttendanceRecords.OrderBy(a => a.Entry).Where(a => a.UserId == user.ID).FirstOrDefault(a => a.Entry > dateStart && (a.Activity?.Property.Count ?? false));
            AttendanceRecord previeousAttendanceRecord = AttendanceRecords.OrderByDescending(a => a.Entry).Where(a => a.UserId == user.ID).FirstOrDefault(a => a.Entry < dateStart && a.Activity.Property.Count);

            List <AttendanceRecord> orderedAttendanceRecord = AttendanceRecords.Where(a => a.UserId == user.ID).Where(a => a.Entry.Date >= (previeousAttendanceRecord?.Entry.Date ?? dateStart) && a.Entry.Date <= (nextAttendanceRecord?.Entry.Date ?? DateTime.Now.Date)).OrderBy(a => a.Entry).ToList();
            List<DateOnly> updatesDays = new List<DateOnly>();

            for (DateTime date = (previeousAttendanceRecord?.Entry.Date ?? dateStart); date <= (nextAttendanceRecord?.Entry.Date ?? DateTime.Now.Date); date = date.AddDays(1))
            {
                updatesDays.Add(DateOnly.FromDateTime(date));
            }


            for (int i = 0; i < orderedAttendanceRecord.Count - 1; i++)
            {
                DateTime startDateTime = orderedAttendanceRecord[i].Entry;
                DateTime endDateTime = orderedAttendanceRecord[i + 1].Entry;
                DateOnly startDate = DateOnly.FromDateTime(startDateTime);
                DateOnly endDate = DateOnly.FromDateTime(endDateTime);

                if (startDate == endDate)
                {
                    attendanceRecordWithEndings.Add(new AttendanceRecordWithEnding(orderedAttendanceRecord[i].User, orderedAttendanceRecord[i].Activity, startDateTime, endDateTime, orderedAttendanceRecord[i].AttendanceRecordDetail));
                }
                else
                {
                    for (DateOnly dateS = startDate; dateS <= endDate; dateS = dateS.AddDays(1))
                    {
                        if (dateS == startDate)
                        {
                            DateTime endOfTheDay = new DateTime(dateS.Year, dateS.Month, dateS.Day, 23, 59, 59);
                            attendanceRecordWithEndings.Add(new AttendanceRecordWithEnding(orderedAttendanceRecord[i].User, orderedAttendanceRecord[i].Activity, startDateTime, endOfTheDay, orderedAttendanceRecord[i].AttendanceRecordDetail));
                        }
                        else if (dateS == endDate)
                        {
                            DateTime startOfTheDay = new DateTime(dateS.Year, dateS.Month, dateS.Day, 0, 0, 0);
                            attendanceRecordWithEndings.Add(new AttendanceRecordWithEnding(orderedAttendanceRecord[i].User, orderedAttendanceRecord[i].Activity, startOfTheDay, endDateTime, orderedAttendanceRecord[i].AttendanceRecordDetail));
                        }
                        else
                        {
                            DateTime startOfTheDay = new DateTime(dateS.Year, dateS.Month, dateS.Day, 0, 0, 0);
                            DateTime endOfTheDay = new DateTime(dateS.Year, dateS.Month, dateS.Day, 23, 59, 59);
                            attendanceRecordWithEndings.Add(new AttendanceRecordWithEnding(orderedAttendanceRecord[i].User, orderedAttendanceRecord[i].Activity, startOfTheDay, endOfTheDay, orderedAttendanceRecord[i].AttendanceRecordDetail));
                        }
                    }
                }
            }

            List<AttendanceTotal> newAttendanceTotalInDay = attendanceRecordWithEndings.Where(a => a.Activity.Property.Count)
                .GroupBy(d => new { ActivityId = d.Activity.ID, Activity = d.Activity, Date = d.Entry.Date, d.Detail })
                .Select(g => new AttendanceTotal
                {
                    UserId = user.ID,
                    Date = DateOnly.FromDateTime(g.Key.Date),
                    Activity = g.Key.Activity,
                    ActivityId = g.Key.ActivityId,
                    Duration = (g.Key.Activity.Property.IsPlan && !g.Key.Activity.Property.HasTime) ? ((g.Key.Detail?.IsHalfDay ?? false) ? _activityStore.GlobalSetting.LenghtOfHalfDayActivity : _activityStore.GlobalSetting.LenghtOfAllDayActivity) : TimeSpan.FromSeconds(g.Sum(e => (e.Exit - e.Entry).TotalSeconds))
                })
                .ToList();

            newAttendanceTotalInDay = newAttendanceTotalInDay.GroupBy(d => new { ActivityId = d.Activity.ID, Activity = d.Activity, Date = d.Date })
                                                            .Select(g => new AttendanceTotal
                                                            {
                                                                UserId = user.ID,
                                                                Date = g.Key.Date,
                                                                Activity = g.Key.Activity,
                                                                ActivityId = g.Key.ActivityId,
                                                                Duration = TimeSpan.FromSeconds(g.Sum(e => (e.Duration).TotalSeconds))
                                                            })
                                                            .ToList();


            //check if there is enough pause for activities
            foreach (DateOnly dateOnly in updatesDays)
            {
                TimeSpan totalPauseInDay = TimeSpan.FromSeconds(newAttendanceTotalInDay.Where(a => a.Date == dateOnly && a.Activity.Property.IsPause).Sum(a => a.Duration.TotalSeconds));
                TimeSpan totalWorkWithPauseInDay = TimeSpan.FromSeconds(newAttendanceTotalInDay.Where(a => a.Date == dateOnly && !a.Activity.Property.IsPause && a.Activity.Property.HasPause).Sum(a => a.Duration.TotalSeconds));

                Activity mainPauseActivity = _activityStore.GlobalSetting.MainPauseActivity;

                if (totalWorkWithPauseInDay >= _activityStore.GlobalSetting.PauseEvery)
                {
                    TimeSpan totalRequiredPause = (int)(totalWorkWithPauseInDay / _activityStore.GlobalSetting.PauseEvery) * _activityStore.GlobalSetting.PauseDuration;

                    if (totalRequiredPause > totalPauseInDay)
                    {
                        //adding required pause
                        AttendanceTotal? pause = newAttendanceTotalInDay.FirstOrDefault(a => a.Date == dateOnly && a.Activity == mainPauseActivity);
                        if (pause != null)
                        {
                            pause.Duration += totalRequiredPause - totalPauseInDay;
                        }
                        else
                        {
                            AttendanceTotal pauseAttendanceTotal = new AttendanceTotal(user, dateOnly, mainPauseActivity, totalRequiredPause - totalPauseInDay);
                            pauseAttendanceTotal.User = null;
                            newAttendanceTotalInDay.Add(pauseAttendanceTotal);
                        }

                        //substract required pause from worked time
                        foreach(AttendanceTotal attendanceTotal in newAttendanceTotalInDay.Where(a => a.Date == dateOnly && a.Activity.Property.Count && a.Activity.Property.IsWork && a.Activity.Property.HasPause))
                        {
                            TimeSpan activityDuration = attendanceTotal.Duration;
                            if (activityDuration > totalRequiredPause)
                            {
                                activityDuration -= totalRequiredPause;
                                break;
                            }
                            else
                            {
                                activityDuration = TimeSpan.Zero;
                                totalRequiredPause -= activityDuration;
                            }
                        }
                    }
                }
            }

            //check if activity isnt reached maxed in day
            //and check plans without time
            foreach (AttendanceTotal attendanceTotal in newAttendanceTotalInDay)
            {
                if (attendanceTotal.Duration > attendanceTotal.Activity.Property.MaxInDay && attendanceTotal.Activity.Property.Count && attendanceTotal.Activity.Property.MaxInDay != TimeSpan.Zero)
                {
                    attendanceTotal.Duration = attendanceTotal.Activity.Property.MaxInDay;
                }

                attendanceTotal.Activity = null;
            }




            await _userDataService.AddAttendanceTotal(user, updatesDays, newAttendanceTotalInDay);
            //_attendanceTotal.RemoveAll(a => a.User == user && updatesDays.Contains(a.Date));
            //_attendanceTotal.AddRange(newAttendanceTotalInDay);

        }

        private long CountDuration(User user, DateOnly date, bool all, bool work = true)
        {
            List<AttendanceTotal> attendanceTotals = AttendanceTotal.Where(a => a.Date == date && a.Activity.Property.Count && (a.Activity.Property.IsWork == work || all)).ToList();
            long CountInDay = (long)attendanceTotals.Sum(a => a.Duration.TotalSeconds);

            List<AttendanceRecord> attendanceRecord = AttendanceRecords.Where(a => a.Entry <= DateTime.Now).ToList();
            if (attendanceRecord.Count > 0)
            {
                AttendanceRecord lastRecord = attendanceRecord.Where(a => a.Entry <= DateTime.Now).OrderBy(a => a.Entry).Last();
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

                    if (lastRecord.Activity.Property.IsPause && lastRecord.Activity.Property.Count)
                    {
                        TimeSpan totalPauseInDay = TimeSpan.FromSeconds(attendanceTotals.Where(a => a.Date == date && a.Activity.Property.IsPause).Sum(a => a.Duration.TotalSeconds));
                        if (totalPauseInDay.TotalSeconds % _activityStore.GlobalSetting.PauseDuration.TotalSeconds == 0)
                        {
                            return CountInDay;
                        }
                    }
                }
            }

            return CountInDay;
        }

        public List<AttendanceRecordItem> RecordsInDay(DateOnly date)
        {
            //List<AttendanceRecord> RecordsInDay = AttendanceRecords.Where(a => DateOnly.FromDateTime(a.Entry) == date).ToList();
            List<AttendanceRecordItem> recordsInDay = AttendanceRecords.Where(a => DateOnly.FromDateTime(a.Entry) == date).OrderByDescending(a => a.Entry).Select(a => new AttendanceRecordItem(a, a.Activity, a.Entry.ToString("HH:mm"))).ToList();

            AttendanceRecord oldRecord = AttendanceRecords.Where(a => a.Entry <= DateTime.Now).Where(a => DateOnly.FromDateTime(a.Entry) < date).OrderBy(a => a.Entry).LastOrDefault();
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

        public List<AttendanceTotal> ActivitiesTotalInDay(DateOnly date)
        {
            List<AttendanceTotal> ActivitiesTotalInDay = new List<AttendanceTotal>();

            List<AttendanceTotal> ActivitiesTotalInDayX = AttendanceTotal.Where(a => a.Date == date && a.Activity.Property.Count).ToList();
            ActivitiesTotalInDay = ActivitiesTotalInDayX.ConvertAll(x => new AttendanceTotal(x.User, x.Date, x.Activity, x.Duration));


            List<AttendanceRecord> attendanceRecord = AttendanceRecords.Where(a => a.Entry <= DateTime.Now).ToList();
            if (attendanceRecord.Count > 0)
            {
                AttendanceRecord lastRecord = attendanceRecord.Where(a => a.Entry <= DateTime.Now).OrderBy(a => a.Entry).Last();
                if (lastRecord.Activity.Property.Count && DateOnly.FromDateTime(lastRecord.Entry) <= date)
                {
                    DateOnly dateS = DateOnly.FromDateTime(lastRecord.Entry);
                    DateOnly now = DateOnly.FromDateTime(DateTime.Now);
                    DateTime startOfTheDay = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                    DateTime endOfTheDay = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                    if (dateS == date && now == date)
                    {
                        AttendanceTotal? AttendanceWithOngoingActivity = ActivitiesTotalInDay.FirstOrDefault(a => a.Activity.ID == lastRecord.Activity.ID);
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
                        AttendanceTotal? AttendanceWithOngoingActivity = ActivitiesTotalInDay.SingleOrDefault(a => a.Activity.ID == lastRecord.Activity.ID);
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
                        AttendanceTotal? AttendanceWithOngoingActivity = ActivitiesTotalInDay.SingleOrDefault(a => a.Activity.ID == lastRecord.Activity.ID);
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
                        AttendanceTotal? AttendanceWithOngoingActivity = ActivitiesTotalInDay.SingleOrDefault(a => a.Activity.ID == lastRecord.Activity.ID);
                        if (AttendanceWithOngoingActivity != null)
                        {
                            AttendanceWithOngoingActivity.Duration += (endOfTheDay - startOfTheDay);
                        }
                        else
                        {
                            ActivitiesTotalInDay.Add(new AttendanceTotal(lastRecord.User, date, lastRecord.Activity, (endOfTheDay - startOfTheDay)));
                        }
                    }

                    if (lastRecord.Activity.Property.IsPause && lastRecord.Activity.Property.Count)
                    {
                        TimeSpan totalPauseInDay = TimeSpan.FromSeconds(ActivitiesTotalInDay.Where(a => a.Date == date && a.Activity.Property.IsPause).Sum(a => a.Duration.TotalSeconds));
                        if (totalPauseInDay.TotalSeconds % _activityStore.GlobalSetting.PauseDuration.TotalSeconds == 0)
                        {
                            return ActivitiesTotalInDay.OrderByDescending(a => a.Duration).ToList();
                        }
                    }
                }
            }

            return ActivitiesTotalInDay.OrderByDescending(a => a.Duration).ToList();
        }

        public async Task<List<AttendanceRecordFix>> GetPendingFixes(User user)
        {
            return await _userDataService.GetPendingAttendanceRecordFixes(user);
        }

        public async Task ApproveFix(AttendanceRecordFix attendanceRecordFix)
        {
            switch (attendanceRecordFix.FixType)
            {
                case FixType.Insert:
                    await AddAttendanceRecord(attendanceRecordFix.User, attendanceRecordFix.Activity, attendanceRecordFix.Entry);
                    break;
                case FixType.Update:
                    await UpdateAttendanceRecord(attendanceRecordFix.User, attendanceRecordFix.Activity, attendanceRecordFix.Entry, attendanceRecordFix.AttendanceRecord);
                    break;
                case FixType.Delete:
                    await RemoveAttendanceRecord(attendanceRecordFix.AttendanceRecord);
                    break;
            }
            attendanceRecordFix.Approved = ApproveType.Approved;
            await _userDataService.FixDecision(attendanceRecordFix, ApproveType.Approved);
            CurrentAttendanceRecordFixChange?.Invoke();
        }

        public async Task RejectFix(AttendanceRecordFix attendanceRecordFix)
        {
            attendanceRecordFix.Approved = ApproveType.Rejected;
            await _userDataService.FixDecision(attendanceRecordFix, ApproveType.Rejected);
            CurrentAttendanceRecordFixChange?.Invoke();
        }

        public List<UsersExportData> LoadUsersExportData(int month, int year, User? user = null)
        {
            return _userDataService.LoadUsersExportData(month, year, user);
        }

        public List<string> LoadHeaderExportData()
        {
            return _userDataService.LoadHeaderExportData();
        }
    }
}
