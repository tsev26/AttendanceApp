using Attendance.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.EF.Services
{
    public class UserDataService
    {
        private readonly DbSQLiteContextFactory _dbContextFactory;
        private readonly NonQueryDataService<User> _nonQueryDataService;

        public UserDataService(DbSQLiteContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            _nonQueryDataService = new NonQueryDataService<User>(dbContextFactory);
        }

        public async Task AddAttendanceRecord(AttendanceRecord attendanceRecord)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                User user = await context.Users.FindAsync(attendanceRecord.User.ID);
                Activity activity = await context.Activities.FindAsync(attendanceRecord.Activity.ID);
                AttendanceRecord attendanceRecordAdd = new AttendanceRecord
                {
                    User = user,
                    UserId = user.ID,
                    Activity = activity,
                    ActivityId = activity.ID,
                    Entry = attendanceRecord.Entry,
                    AttendanceRecordDetail = attendanceRecord.AttendanceRecordDetail,
                    AttendanceRecordDetailInt = attendanceRecord.AttendanceRecordDetailInt
                };

                context.AttendanceRecords.Add(attendanceRecordAdd);
                await context.SaveChangesAsync();
                return;
            }
        }

        public async Task AddAttendanceRecordFix(AttendanceRecordFix attendanceRecordFix)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                attendanceRecordFix.User = null;
                attendanceRecordFix.Activity = null;
                attendanceRecordFix.AttendanceRecord = null;
                context.AttendanceRecordFixes.Add(attendanceRecordFix);
                await context.SaveChangesAsync();
                return;
            }
        }
    

        public async Task AddAttendanceTotal(User user, List<DateOnly> updatesDays, List<AttendanceTotal> newAttendanceTotalInDay)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                List<AttendanceTotal> attendaceTotalsToRemove = context.AttendanceTotals.Where(a => a.User == user && updatesDays.Contains(a.Date)).ToList();
                context.AttendanceTotals.RemoveRange(attendaceTotalsToRemove);
                context.AttendanceTotals.AddRange(newAttendanceTotalInDay);
                await context.SaveChangesAsync();
                return;
            }
        }

        public async Task<List<AttendanceRecordFix>> GetAttendanceRecordFixes(User user)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.AttendanceRecordFixes
                                    .Where(a => a.User == user)
                                    .Include(a => a.Activity)
                                    .ThenInclude(a => a.Property)
                                    .AsNoTracking()
                                    .ToListAsync();
            }
        }

        public async Task<List<AttendanceRecord>> GetAttendanceRecords(User user)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.AttendanceRecords
                                    .Where(a => a.User == user)
                                    .Include(a => a.Activity)
                                    .ThenInclude(a => a.Property)
                                    .Include(a => a.AttendanceRecordDetail)
                                    .AsNoTracking()
                                    .ToListAsync();
            }
        }

        public async Task<List<AttendanceTotal>> GetAttendanceTotals(User user)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.AttendanceTotals
                                    .Where(a => a.User == user)
                                    .Include(a => a.Activity)
                                    .ThenInclude(a => a.Property)
                                    .AsNoTracking()
                                    .ToListAsync();
            }
        }

        public async Task<User?> GetUserByKey(string key)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                User user = await context.Users.AsNoTracking()
                                         .FirstOrDefaultAsync(a => a.Keys.Any(c => c.KeyValue.ToUpper() == key) && a.ToApprove == false);
                return user;
            }
        }

        public async Task<bool> IsSupervisor(User? user)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                List<User> users = await context.Users.Include(a => a.Group).ThenInclude(s => s.Supervisor).AsNoTracking().ToListAsync();
                bool value = users.Any(a => a.IsSubordinate(user));
                return value;
            }
        }

        public async Task<User> LoadUserData(User user)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                User userData = await context.Users
                                         .Include(a => a.Group)
                                            .ThenInclude(g => g.Obligation)
                                                .ThenInclude(o => o.AvailableActivities)
                                                    .ThenInclude(p => p.Property)
                                         .Include(a => a.Obligation)
                                            .ThenInclude(o => o.AvailableActivities)
                                                .ThenInclude(p => p.Property)
                                         .Include(a => a.Keys)
                                         .Include(a => a.AttendanceRecords)
                                            .ThenInclude(r => r.Activity)
                                         .Include(a => a.AttendanceRecords)
                                            .ThenInclude(r => r.AttendanceRecordDetail)
                                         .Include(a => a.AttendanceTotals)
                                            .ThenInclude(t => t.Activity)
                                        .AsNoTracking()
                                        .FirstAsync(a => a.ID == user.ID);
                return userData;
            }
        }

        public async Task RemoveAttendanceRecord(AttendanceRecord attendanceRecord)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                context.AttendanceRecords.RemoveRange(attendanceRecord);
                await context.SaveChangesAsync();
                return;
            }
        }
    }
}
