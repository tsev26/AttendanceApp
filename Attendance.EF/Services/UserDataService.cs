using Attendance.Domain.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<User?> GetUserByKey(string key)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                User user = await context.Users
                                         .FirstOrDefaultAsync(a => a.Keys.Any(c => c.KeyValue.ToUpper() == key) && a.ToApprove == false);
                return user;
            }
        }

        public async Task<bool> IsSupervisor(User? user)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                List<User> users = await context.Users.Include(a => a.Group).ThenInclude(s => s.Supervisor).ToListAsync();
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
                                        .FirstAsync(a => a.ID == user.ID);
                return userData;
            }
        }
    }
}
