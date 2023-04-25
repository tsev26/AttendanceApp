using Attendance.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
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

        public async Task AddActivityToGroup(Group group, Activity activity)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    Activity activityToAdd = await context.Activities.FindAsync(activity.ID);
                    Group groupToAddActivity = await context.Groups.Include(a => a.AvailableActivities).FirstOrDefaultAsync(a => a.Name == group.Name);
                    groupToAddActivity.AvailableActivities.Add(activityToAdd);

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

        public async Task AddAttendanceRecord(AttendanceRecord attendanceRecord)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
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
            }
        }
    

        public async Task AddAttendanceTotal(User user, List<DateOnly> updatesDays, List<AttendanceTotal> newAttendanceTotalInDay)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    List<AttendanceTotal> attendaceTotalsToRemove = context.AttendanceTotals.Where(a => a.User == user && updatesDays.Contains(a.Date)).ToList();
                    context.AttendanceTotals.RemoveRange(attendaceTotalsToRemove);
                    context.AttendanceTotals.AddRange(newAttendanceTotalInDay);
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

        public async Task AddGroups(Group newGroup)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    Group group = new Group(newGroup);
                    group.SupervisorId = group.Supervisor.ID;
                    group.Supervisor = null;
                    group.Members = new List<User>();
                    context.Groups.Add(group);

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

        public async Task<User?> AddUser(User newUser)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    if (!context.Users.Contains(newUser))
                    {
                        User userToApprove = new User()
                        {
                            UserUpdateId = newUser.UserUpdateId,
                            FirstName = newUser.FirstName,
                            LastName = newUser.LastName,
                            Email = newUser.Email,
                            IsAdmin = newUser.IsAdmin,
                            ToApprove = newUser.ToApprove,
                            IsFastWorkSet = newUser.IsFastWorkSet,
                            Keys = new List<Key>(),
                            AttendanceRecords = new List<AttendanceRecord>(),
                            AttendanceTotals = new List<AttendanceTotal>()
                        };

                        if (newUser.Obligation != null)
                        {
                            userToApprove.Obligation = new Obligation()
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
                                WorksSunday = newUser.Obligation.WorksSunday
                            };
                        }
                        if (newUser.UserUpdateId == null)
                        {
                            if (newUser.Group != null)
                            {
                                userToApprove.GroupId = newUser.Group.ID;
                            }

                            if (newUser.Keys != null)
                            {
                                userToApprove.Keys = newUser.Keys;
                            }
                        }
                        

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
                try
                {
                    return await context.AttendanceRecordFixes
                                        .Where(a => a.User == user)
                                        .Include(a => a.Activity)
                                        .ThenInclude(a => a.Property)
                                        .AsNoTracking()
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

        public async Task<List<AttendanceRecord>> GetAttendanceRecords(User user)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    return await context.AttendanceRecords
                                    .Where(a => a.User == user)
                                    .Include(a => a.Activity)
                                    .ThenInclude(a => a.Property)
                                    .Include(a => a.AttendanceRecordDetail)
                                    .AsNoTracking()
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

        public async Task<List<AttendanceTotal>> GetAttendanceTotals(User user)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    return await context.AttendanceTotals
                                    .Where(a => a.User == user)
                                    .Include(a => a.Activity)
                                    .ThenInclude(a => a.Property)
                                    .AsNoTracking()
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

        public async Task<List<Group>> GetGroups()
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    var list = await context.Groups
                                            .Include(a => a.Obligation)
                                            .Include(a => a.Supervisor)
                                            .Include(a => a.AvailableActivities)
                                            .Include(a => a.Members)
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
                    User user = await context.Users.Where(a => a.ID == userId && !a.ToApprove)
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

        public async Task<List<User>> GetUsers(User? user = null)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    List<User> users = await context.Users.Where(a => !a.ToApprove)
                                                    .Include(a => a.AttendanceRecords)
                                                    .Include(a => a.Group)
                                                        .ThenInclude(a => a.Obligation)
                                                    .Include(a => a.Obligation)
                                                    .Include(a => a.Keys)
                                                    .Where(a => (user == null) || (a.Group.SupervisorId == user.ID || user.IsAdmin))
                                                    .ToListAsync();
                    return users;
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

        public async Task<List<UsersCurrentActivity>> GetUsersCurrentActivities()
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    List<UsersCurrentActivity> result = await context.Users
                        .Where(u => !u.ToApprove)
                        .Include(u => u.AttendanceRecords)
                            .ThenInclude(a => a.Activity)
                        .Select(u => new UsersCurrentActivity
                        {
                            User = u,
                            LastAttendanceRecord = u.AttendanceRecords.Where(ar => ar.Entry < DateTime.Now)
                                                                      .OrderByDescending(ar => ar.Entry)
                                                                      .FirstOrDefault()
                        })
                        .ToListAsync();
                    return result;
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
                try
                {
                    List<User> users = await context.Users.Include(a => a.Group).ThenInclude(s => s.Supervisor).AsNoTracking().ToListAsync();
                    bool value = users.Any(a => a.Group.Supervisor == user || user.IsAdmin);
                    return value;
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

        public async Task<User> LoadUserData(User user)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    User userData = await context.Users
                         .Include(a => a.Group)
                            .ThenInclude(g => g.Obligation)
                         .Include(a => a.Group)
                                .ThenInclude(o => o.AvailableActivities)
                                    .ThenInclude(p => p.Property)
                         .Include(a => a.Obligation)
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

        public async Task<List<User>> LoadUserProfileFixes(User user)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    List<User> users = await context.Users
                                                .Include(a => a.Obligation)
                                                .Where(a => a.UserUpdateId == user.ID && a.ToApprove)
                                                .ToListAsync();
                    return users;
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

        public async Task RemoveActivityFromGroup(Group group, Activity activity)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    Activity activityToRemove = await context.Activities.FindAsync(activity.ID);
                    Group groupToRemoveActivity = await context.Groups.Include(a => a.AvailableActivities).FirstOrDefaultAsync(a => a.ID == group.ID);
                    groupToRemoveActivity.AvailableActivities.Remove(activityToRemove);

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

        public async Task RemoveGroups(Group group)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    List<User> users = await context.Users.Include(a => a.Group).Where(a => a.Group.Name == group.Name).ToListAsync();
                    if (users.Count > 0)
                    {
                        Group unassignedGroup = await context.Groups.Where(a => a.Name == "Nezařazení").FirstOrDefaultAsync();

                        if (unassignedGroup == null)
                        {
                            unassignedGroup = new Group("Nezařazení", group.Supervisor);
                            unassignedGroup.SupervisorId = group.SupervisorId;
                            unassignedGroup.Supervisor = null;
                            unassignedGroup.AvailableActivities = new List<Activity>();
                            unassignedGroup.Members = users;
                        }

                        foreach (User user in users)
                        {
                            user.Group = unassignedGroup;
                        }

                        await context.SaveChangesAsync();
                    }

                    Group groupToRemove = await context.Groups.Where(a => a.Name == group.Name).FirstOrDefaultAsync();
                    groupToRemove.Supervisor = null; 

                    context.Groups.Remove(groupToRemove);

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

        public async Task SetSupervisorToGroup(User? user, Group group)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    Group groupToSetSupervisor = await context.Groups.FirstOrDefaultAsync(a => a.Name == group.Name);
                    groupToSetSupervisor.SupervisorId = user.ID;

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

        public async Task UpdateGroup(Group group)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    Group groupToUpdate = await context.Groups.Include(a => a.Obligation).FirstOrDefaultAsync(a => a.ID == group.ID);
                    groupToUpdate.Obligation.MinHoursWorked = group.Obligation.MinHoursWorked;
                    groupToUpdate.Obligation.HasRegularWorkingTime = group.Obligation.HasRegularWorkingTime;
                    groupToUpdate.Obligation.LatestArival = group.Obligation.LatestArival;
                    groupToUpdate.Obligation.EarliestDeparture = group.Obligation.EarliestDeparture;
                    groupToUpdate.Obligation.WorksMonday = group.Obligation.WorksMonday;
                    groupToUpdate.Obligation.WorksTuesday = group.Obligation.WorksTuesday;
                    groupToUpdate.Obligation.WorksWednesday = group.Obligation.WorksWednesday;
                    groupToUpdate.Obligation.WorksThursday = group.Obligation.WorksThursday;
                    groupToUpdate.Obligation.WorksFriday = group.Obligation.WorksFriday;
                    groupToUpdate.Obligation.WorksSaturday = group.Obligation.WorksSaturday;
                    groupToUpdate.Obligation.WorksSunday = group.Obligation.WorksSunday;

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

        public async Task UpdateUser(User user)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    int id = user.UserUpdateId ?? user.ID;
                    User userToUpdate = await context.Users.Where(a => a.ID == id).Include(a => a.Obligation).Include(a => a.Group).ThenInclude(a => a.Obligation).FirstOrDefaultAsync();
                    userToUpdate.FirstName = user.FirstName;
                    userToUpdate.LastName = user.LastName;
                    userToUpdate.Email = user.Email;
                    if (userToUpdate.Obligation == null)
                    {
                        userToUpdate.Obligation = new Obligation();
                    }

                    Obligation obligation = user.Obligation;
                    if (obligation != null)
                    {
                        userToUpdate.Obligation.HasRegularWorkingTime = obligation.HasRegularWorkingTime;
                        userToUpdate.Obligation.MinHoursWorked = obligation.MinHoursWorked;
                        userToUpdate.Obligation.LatestArival = obligation.LatestArival;
                        userToUpdate.Obligation.EarliestDeparture = obligation.EarliestDeparture;
                        userToUpdate.Obligation.WorksMonday = obligation.WorksMonday;
                        userToUpdate.Obligation.WorksTuesday = obligation.WorksTuesday;
                        userToUpdate.Obligation.WorksWednesday = obligation.WorksWednesday;
                        userToUpdate.Obligation.WorksThursday = obligation.WorksThursday;
                        userToUpdate.Obligation.WorksFriday = obligation.WorksFriday;
                        userToUpdate.Obligation.WorksSaturday = obligation.WorksSaturday;
                        userToUpdate.Obligation.WorksSunday = obligation.WorksSunday;
                    }
                    else
                    {
                        userToUpdate.Obligation = null;
                    }


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

        public async Task UserSetGroup(User user, Group group)
        {
            using (DatabaseContext context = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    User userToAddToGroup = await context.Users.FindAsync(user.ID);
                    userToAddToGroup.GroupId = group.ID;

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
