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
                try
                {
                    ActivityGlobalSetting activityGlobalSetting = context.ActivityGlobalSetting
                                                                         .Include(a => a.MainWorkActivity)
                                                                         .ThenInclude(a => a.Property)
                                                                         .Include(a => a.MainNonWorkActivity)
                                                                         .ThenInclude(a => a.Property)
                                                                         .Include(a => a.MainPauseActivity)
                                                                         .ThenInclude(a => a.Property)
                                                                         .AsNoTracking()
                                                                         .FirstOrDefault();
                    return activityGlobalSetting;
                }
                catch (Exception ex)
                {
                    var errorMsg = $"Error occurred while saving data to the database: {ex.Message}";
                    errorMsg += $" Inner exception: {ex.InnerException}";
                    // log the error or display the error message to the user
                    Console.WriteLine(errorMsg);
                }
                return null;
            }
        }

        public async Task<List<Activity>> GetActivities()
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    List<Activity> activities = await context.Activities
                                                       .Include(a => a.Property)
                                                       .AsNoTracking()
                                                       .ToListAsync();
                    return activities;
                }
                catch (Exception ex)
                {
                    var errorMsg = $"Error occurred while saving data to the database: {ex.Message}";
                    errorMsg += $" Inner exception: {ex.InnerException}";
                    // log the error or display the error message to the user
                    Console.WriteLine(errorMsg);
                }
                return null;
            }
        }

        public async Task AddActivity(Activity activity)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    context.Activities.Add(activity);

                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var errorMsg = $"Error occurred while saving data to the database: {ex.Message}";
                    errorMsg += $" Inner exception: {ex.InnerException}";
                    // log the error or display the error message to the user
                    Console.WriteLine(errorMsg);
                }
            }
        }

        public async Task UpdateActivity(Activity activity)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    Activity activityToUpdate = await context.Activities.Where(a => a.ID == activity.ID).Include(a => a.Property).FirstOrDefaultAsync();
                    activityToUpdate.Name = activity.Name;
                    activityToUpdate.Shortcut = activity.Shortcut;
                    activityToUpdate.Property.IsPlan = activity.Property.IsPlan;
                    activityToUpdate.Property.Count = activity.Property.Count;
                    activityToUpdate.Property.IsPause = activity.Property.IsPause;
                    activityToUpdate.Property.HasPause = activity.Property.HasPause;
                    activityToUpdate.Property.HasTime = activity.Property.HasTime;
                    activityToUpdate.Property.MaxInDay = activity.Property.MaxInDay;
                    activityToUpdate.Property.GroupByName = activity.Property.GroupByName;


                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var errorMsg = $"Error occurred while saving data to the database: {ex.Message}";
                    errorMsg += $" Inner exception: {ex.InnerException}";
                    // log the error or display the error message to the user
                    Console.WriteLine(errorMsg);
                }
            }
        }

        public async Task UpdateGlobalSetting(ActivityGlobalSetting activityGlobalSetting)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    ActivityGlobalSetting activityGlobalSettingToUpdate = await context.ActivityGlobalSetting
                                                                                       .Where(a => a.ID == activityGlobalSetting.ID)
                                                                                       .FirstOrDefaultAsync();
                    activityGlobalSettingToUpdate.PauseEvery = activityGlobalSetting.PauseEvery;
                    activityGlobalSettingToUpdate.PauseDuration = activityGlobalSetting.PauseDuration;
                    activityGlobalSettingToUpdate.MainWorkActivityId = activityGlobalSetting.MainWorkActivity.ID;
                    activityGlobalSettingToUpdate.MainPauseActivityId = activityGlobalSetting.MainPauseActivity.ID;
                    activityGlobalSettingToUpdate.MainNonWorkActivityId = activityGlobalSetting.MainNonWorkActivity.ID;
                    activityGlobalSettingToUpdate.LenghtOfAllDayActivity = activityGlobalSetting.LenghtOfAllDayActivity;
                    activityGlobalSettingToUpdate.LenghtOfHalfDayActivity = activityGlobalSetting.LenghtOfHalfDayActivity;


                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var errorMsg = $"Error occurred while saving data to the database: {ex.Message}";
                    errorMsg += $" Inner exception: {ex.InnerException}";
                    // log the error or display the error message to the user
                    Console.WriteLine(errorMsg);
                }
            }
        }
    }
}
