using Attendance.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.EF.Services
{
    public class ActivityDataService
    {
        private readonly DbSQLiteContextFactory _dbContextFactory;
        private readonly NonQueryDataService<Activity> _nonQueryDataService;

        public ActivityDataService(DbSQLiteContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            _nonQueryDataService = new NonQueryDataService<Activity>(dbContextFactory);
        }

        public ActivityGlobalSetting GetActivityGlobalSetting()
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                ActivityGlobalSetting activityGlobalSetting = context.ActivityGlobalSetting
                                                                     .Include(a => a.MainWorkActivity)
                                                                     .Include(a => a.MainNonWorkActivity)
                                                                     .Include(a => a.MainPauseActivity)
                                                                     .FirstOrDefault();
                return activityGlobalSetting;
            }
        }

        public async Task<List<Activity>> GetActivities()
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                List<Activity> activities = await context.Activities
                                                   .Include(a => a.Property)
                                                   .ToListAsync();
                return activities;
            }
        }
    }
}
