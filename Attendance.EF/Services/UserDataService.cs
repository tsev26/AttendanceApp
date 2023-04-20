using Attendance.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Key = Attendance.Domain.Models.Key;

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
                try
                {
                    AttendanceRecordFix attendanceRecordFixNew = new AttendanceRecordFix()
                    {
                        AttendanceRecordId = attendanceRecordFix.AttendanceRecordId,
                        ActivityId = attendanceRecordFix.ActivityId,
                        UserId = attendanceRecordFix.UserId,
                        Entry = attendanceRecordFix.Entry,
                        FixType = attendanceRecordFix.FixType,
                        Approved = attendanceRecordFix.Approved
                    };

                    context.AttendanceRecordFixes.Add(attendanceRecordFixNew);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var errorMsg = $"Error occurred while saving data to the database: {ex.Message}";
                    errorMsg += $" Inner exception: {ex.InnerException}";
                    // log the error or display the error message to the user
                    Console.WriteLine(errorMsg);
                }
                catch (DbUpdateException ex)
                {
                    var errorMsg = $"Error occurred while saving data to the database: {ex.Message}";
                    errorMsg += $" Inner exception: {ex.InnerException}";
                    // log the error or display the error message to the user
                    Console.WriteLine(errorMsg);
                }

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

        public async Task<User?> AddUser(User newUser)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    if (context.Users.Contains(newUser))
                    {
                        User userToApprove = new User()
                        {
                            UserId = newUser.UserId,
                            FirstName = newUser.FirstName,
                            LastName = newUser.LastName,
                            Email = newUser.Email,
                            IsAdmin = newUser.IsAdmin,
                            ToApprove = newUser.ToApprove,
                            IsFastWorkSet = newUser.IsFastWorkSet,
                            Keys = new List<Key>(),
                            AttendanceRecords = new List<AttendanceRecord>(),
                            AttendanceTotals = new List<AttendanceTotal>(),
                            Obligation = new Obligation()
                            {
                                MinHoursWorked = newUser.Obligation.MinHoursWorked,
                                HasRegularWorkingTime = newUser.Obligation.HasRegularWorkingTime,
                                LatestArival = newUser.Obligation.LatestArival,
                                EarliestDeparture = newUser.Obligation.EarliestDeparture,
                                WorksMonday = newUser.Obligation.WorksMonday,
                                WorksTuesday = newUser.Obligation.WorksTuesday,
                                WorksWednesday = newUser.Obligation.WorksWednesday,
                                WorksThursday = newUser.Obligation.WorksThursday,
                                WorksFriday = newUser.Obligation.WorksFriday,
                                WorksSaturday = newUser.Obligation.WorksSaturday,
                                WorksSunday = newUser.Obligation.WorksSunday,
                                AvailableActivities = new List<Activity>()
                            }
                        };
                        context.Users.Add(userToApprove);
                        await context.SaveChangesAsync();
                        return userToApprove;
                    }                    
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

        public async Task FixDecision(AttendanceRecordFix attendanceRecordFix, ApproveType decision)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    AttendanceRecordFix fix = await context.AttendanceRecordFixes.Where(a => a.ID == attendanceRecordFix.ID).FirstOrDefaultAsync();
                    fix.Approved = decision;
                    await context.SaveChangesAsync();
                    return;
                }
                catch (Exception ex)
                {
                    var errorMsg = $"Error occurred while saving data to the database: {ex.Message}";
                    errorMsg += $" Inner exception: {ex.InnerException}";
                    // log the error or display the error message to the user
                    Console.WriteLine(errorMsg);
                }
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

        public async Task<List<AttendanceRecordFix>> GetPendingAttendanceRecordFixes(User user)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    var list = await context.AttendanceRecordFixes
                                              .Include(a => a.Activity)
                                                .ThenInclude(a => a.Property)
                                              .Include(a => a.User)
                                              .ThenInclude(a => a.Group)
                                              .Include(a => a.AttendanceRecord)
                                              .Where(a => a.Approved == ApproveType.Waiting && (a.User.Group.SupervisorId == user.ID || user.IsAdmin))
                                              .ToListAsync();
                    return list;
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

        public async Task<List<User>> GetPendingProfileUpdates(User? user)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    return await context.Users.Where(a => a.ToApprove && (a.Group.Supervisor.ID == user.ID || user.IsAdmin))
                                              .Include(a => a.Obligation)
                                              .Include(a => a.Group)
                                                .ThenInclude(a => a.Obligation)
                                              .ToListAsync();
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

        public async Task<User?> GetUserByKey(string key)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                User user = await context.Users.AsNoTracking()
                                         .FirstOrDefaultAsync(a => a.Keys.Any(c => c.KeyValue.ToUpper() == key) && a.ToApprove == false);
                return user;
            }
        }

        public async Task<User> GetUserByUserId(int userId)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    User user = await context.Users.Where(a => a.UserId == userId && !a.ToApprove)
                                                   .Include(a => a.Obligation)
                                                   .Include(a => a.Group)
                                                   .ThenInclude(a => a.Obligation)
                                                   .FirstOrDefaultAsync();
                    return user;
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

        public Task<List<User>> LoadUserProfileFixes(User user)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                return context.Users.Include(a => a.Obligation).Where(a => a.UserId == user.UserId && a.ToApprove).ToListAsync();
            }
        }

   

        public async Task RemoveAttendanceRecord(AttendanceRecord attendanceRecord)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    
                    User userWithRecordToDelete = await context.Users.Include(a => a.AttendanceRecords).Where(a => a.AttendanceRecords.Contains(attendanceRecord)).FirstOrDefaultAsync();
                    userWithRecordToDelete.AttendanceRecords.Remove(attendanceRecord);

                    AttendanceRecord attendanceRecordToDelete = await context.AttendanceRecords.Where(a => a.ID == attendanceRecord.ID).FirstOrDefaultAsync();
                    attendanceRecordToDelete.User = null;
                    context.AttendanceRecords.Remove(attendanceRecordToDelete);
                    await context.SaveChangesAsync();
                    return;
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

        public async Task RemoveKey(Key KeyValueToDelete)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    context.Keys.Remove(KeyValueToDelete);
                    await context.SaveChangesAsync();
                    return;
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

        public async Task RemoveUser(User deleteUser)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    User userToDelete = await context.Users.Where(a => a.ID == deleteUser.ID).FirstOrDefaultAsync();
                    context.Users.Remove(userToDelete);
                    await context.SaveChangesAsync();
                    return;
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

        public async Task SetFastWork(User selectedUser, bool isFastWorkSet)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    User userToUpdateFastWorkSet = await context.Users.Where(a => a.ID == selectedUser.ID).FirstOrDefaultAsync();
                    userToUpdateFastWorkSet.IsFastWorkSet = isFastWorkSet;
                    await context.SaveChangesAsync();
                    return;
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

        public async Task UpdateAttendanceRecord(AttendanceRecord newAttendanceRecord, AttendanceRecord oldAttendanceRecord)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    AttendanceRecord attendanceRecordToUpdate = await context.AttendanceRecords.Where(a => a.ID == oldAttendanceRecord.ID).FirstOrDefaultAsync();
                    attendanceRecordToUpdate.Entry = newAttendanceRecord.Entry;
                    attendanceRecordToUpdate.ActivityId = newAttendanceRecord.ActivityId;
                    attendanceRecordToUpdate.AttendanceRecordDetailInt = newAttendanceRecord.AttendanceRecordDetailInt;
                    await context.SaveChangesAsync();
                    return;
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

        public async Task UpdateUser(User user)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    User userToUpdate = await context.Users.Where(a => a.UserId == user.UserId).Include(a => a.Obligation).Include(a => a.Group).ThenInclude(a => a.Obligation).ThenInclude(a => a.AvailableActivities).FirstOrDefaultAsync();
                    userToUpdate.FirstName = user.FirstName;
                    userToUpdate.LastName = user.LastName;
                    userToUpdate.Email = user.Email;
                    if (userToUpdate.Obligation == null)
                    {
                        userToUpdate.Obligation = new Obligation();
                    }
                    userToUpdate.Obligation.HasRegularWorkingTime = user.Obligation.HasRegularWorkingTime;
                    userToUpdate.Obligation.MinHoursWorked = user.Obligation.MinHoursWorked;
                    userToUpdate.Obligation.LatestArival = user.Obligation.LatestArival;
                    userToUpdate.Obligation.EarliestDeparture = user.Obligation.EarliestDeparture;
                    userToUpdate.Obligation.WorksMonday = user.Obligation.WorksMonday;
                    userToUpdate.Obligation.WorksTuesday = user.Obligation.WorksTuesday;
                    userToUpdate.Obligation.WorksWednesday = user.Obligation.WorksWednesday;
                    userToUpdate.Obligation.WorksThursday = user.Obligation.WorksThursday;
                    userToUpdate.Obligation.WorksFriday = user.Obligation.WorksFriday;
                    userToUpdate.Obligation.WorksSaturday = user.Obligation.WorksSaturday;
                    userToUpdate.Obligation.WorksSunday = user.Obligation.WorksSunday;
                    userToUpdate.Obligation.AvailableActivities = new List<Activity>();

                    await context.SaveChangesAsync();
                    return;
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

        public async Task<bool> UpsertKey(User user, Key newKeyValue)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    newKeyValue.UserId = user.ID;
                    if (await context.Users.AnyAsync(a => a.Keys.Any(a => a.KeyValue == newKeyValue.KeyValue)))
                    {
                        return false;
                    }
                    Key? existingKey = context.Keys.FirstOrDefault(a => a.ID == newKeyValue.ID);
                    if (existingKey != null)
                    {
                        existingKey.KeyValue = newKeyValue.KeyValue;
                    }
                    else
                    {
                        context.Keys.Add(newKeyValue);
                    }
                    await context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    var errorMsg = $"Error occurred while saving data to the database: {ex.Message}";
                    errorMsg += $" Inner exception: {ex.InnerException}";
                    // log the error or display the error message to the user
                    Console.WriteLine(errorMsg);
                }
                return false;
            }
        }
    }
}
