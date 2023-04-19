using Attendance.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Attendance.EF
{
    public class DbSQLiteContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        private string _connectionString;
        private static string _fileName = "appsettings.json";
        private string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _fileName);
        public DbSQLiteContextFactory() { }

        class ConnectionStrings
        {
            public string SQLite { get; set; }
        }

        class ConnectionStringsObject
        {
            public ConnectionStrings ConnectionStrings { get; set; }
        }
        public bool InitDbSQLite()
        {
            string jsonString;
            if (!File.Exists(_filePath))
            {
                using (StreamWriter writer = File.CreateText(_filePath))
                {
                    var connectionStringsObject = new ConnectionStringsObject
                    {
                        ConnectionStrings = new ConnectionStrings
                        {
                            SQLite = ""
                        }
                    };

                    jsonString = JsonSerializer.Serialize(connectionStringsObject);

                    writer.Write(jsonString);
                }
            }

            jsonString = File.ReadAllText(_filePath);

            var jsonObject = JsonSerializer.Deserialize<ConnectionStringsObject>(jsonString);

            _connectionString = jsonObject.ConnectionStrings.SQLite;

            return File.Exists(_connectionString);
        }


        public DatabaseContext CreateDbContext(string[] args = null)
        {
            DbContextOptions<DatabaseContext> options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlite($"Data Source={_connectionString}")
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            .Options;

            DatabaseContext database = new DatabaseContext(options);
            return database;
        }

        public void WriteDbConnection()
        {
            string jsonString = File.ReadAllText(_filePath);

            var jsonObject = JsonSerializer.Deserialize<ConnectionStringsObject>(jsonString);

            jsonObject.ConnectionStrings.SQLite = _connectionString;

            string updatedJsonString = JsonSerializer.Serialize(jsonObject, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(_filePath, updatedJsonString);
        }

        public void SetDbPath(string dbPath)
        {
            _connectionString = dbPath;
            WriteDbConnection();


            if (!File.Exists(_connectionString))
            {
                using (var context = CreateDbContext())
                {
                    context.Database.EnsureCreated();
                }

            }

            // Merge the DbContext into the existing database
            using (var context = CreateDbContext())
            {
                context.Database.Migrate();
            }
        }

        public void SetDefaultValuesIntoDb()
        {
            bool dbExists = false;
            using (DatabaseContext dbContext = CreateDbContext())
            {
                dbExists = dbContext.Database.CanConnect();

                // create some default values 
                if (dbExists && !dbContext.Activities.Any())
                {
                    ActivityProperty workActivityProperty = new ActivityProperty(false, true, false, true, false, new TimeSpan(15, 0, 0), "práce");
                    Activity workActivity = new Activity("Práce", "P", workActivityProperty);
                    ActivityProperty pauseActivityProperty = new ActivityProperty(false, true, true, false, false, new TimeSpan(0, 0, 0), "pauza");
                    Activity pauseActivity = new Activity("Pauza", "A", pauseActivityProperty);
                    ActivityProperty homeActivityProperty = new ActivityProperty(false, false, false, false, false, new TimeSpan(0, 0, 0), "domov");
                    Activity homeActivity = new Activity("Domov", "D", homeActivityProperty);
                    ActivityProperty btActivityProperty = new ActivityProperty(true, true, false, true, true, new TimeSpan(10, 0, 0), "práce");
                    Activity btActivity = new Activity("Služební cesta", "S", btActivityProperty);
                    ActivityProperty vacationActivityProperty = new ActivityProperty(true, true, false, false, false, new TimeSpan(12, 0, 0), "dovolená");
                    Activity vacationActivity = new Activity("Dovolená", "O", vacationActivityProperty);

                    using (DatabaseContext dbContextAdd = CreateDbContext())
                    {
                        dbContextAdd.Activities.Add(workActivity);
                        dbContextAdd.Activities.Add(pauseActivity);
                        dbContextAdd.Activities.Add(homeActivity);
                        dbContextAdd.Activities.Add(btActivity);
                        dbContextAdd.Activities.Add(vacationActivity);

                        dbContextAdd.SaveChanges();
                    }    
                }

                if (dbExists && !dbContext.ActivityGlobalSetting.Any())
                {
                    using (DatabaseContext dbContextAdd = CreateDbContext())
                    {
                        Activity workActivity = dbContextAdd.Activities.FirstOrDefault(a => a.Name == "Práce");
                        Activity pauseActivity = dbContextAdd.Activities.FirstOrDefault(a => a.Name == "Pauza");
                        Activity homeActivity = dbContextAdd.Activities.FirstOrDefault(a => a.Name == "Domov");
                        ActivityGlobalSetting activityGlobalSetting = new ActivityGlobalSetting(new TimeSpan(6, 0, 0), new TimeSpan(0, 30, 0), workActivity, pauseActivity, homeActivity, new TimeSpan(8, 0, 0), new TimeSpan(4, 0, 0));
                        dbContextAdd.ActivityGlobalSetting.Add(activityGlobalSetting);

                        dbContextAdd.SaveChanges();
                    }
                    
                }

                if (dbExists && !dbContext.Users.Any())
                {
                    using (DatabaseContext dbContextAdd = CreateDbContext())
                    {
                        
                        Activity workActivity = dbContextAdd.Activities.FirstOrDefault(a => a.Name == "Práce");
                        Activity pauseActivity = dbContextAdd.Activities.FirstOrDefault(a => a.Name == "Pauza");
                        Activity homeActivity = dbContextAdd.Activities.FirstOrDefault(a => a.Name == "Domov");
                        Activity btActivity = dbContextAdd.Activities.FirstOrDefault(a => a.Name == "Služební cesta");
                        Activity vacationActivity = dbContextAdd.Activities.FirstOrDefault(a => a.Name == "Dovolená");

                        List<Activity> activitiesBasic = new List<Activity>();
                        activitiesBasic.Add(workActivity);
                        activitiesBasic.Add(homeActivity);
                        activitiesBasic.Add(pauseActivity);

                        List<Activity> activitiesAll = new List<Activity>();
                        activitiesAll.Add(workActivity);
                        activitiesAll.Add(homeActivity);
                        activitiesAll.Add(pauseActivity);
                        activitiesAll.Add(btActivity);
                        activitiesAll.Add(vacationActivity);
                        
                        Obligation obligation = new Obligation(8, true, new TimeOnly(9, 0, 0), new TimeOnly(15, 0, 0), true, true, true, true, true, false, false, activitiesBasic);
                        Obligation obligationVedeni = new Obligation(6, true, new TimeOnly(12, 0, 0), new TimeOnly(13, 0, 0), true, true, false, true, true, false, false, activitiesAll);
                        
                        Group leadershipGroup = new Group("Vedení", null);
                        Group basicGroup = new Group("Základní", null);
                        leadershipGroup.Obligation = obligationVedeni;
                        basicGroup.Obligation = obligation;


                        User admin = new User("admin", "admin", "tsevcu@gmail.com", true);
                        admin.Group = leadershipGroup;
                        admin.AddKey("aaa");
                        admin.IsAdmin = true;

                        User user1 = new User("Tomáš", "Ševců", "tsevcu@gmail.com", false);
                        user1.Group = leadershipGroup;
                        user1.AddKey("tse");


                        User user2 = new User("TEST", "8", "tsevcu@gmail.com", false);
                        user2.AddKey("test");
                        user2.Group = basicGroup;
                        User user3 = new User("Petr", "Ševců", "tsevcu@gmail.com", false);
                        user3.AddKey("etr");
                        user3.Group = basicGroup;
                        User user4 = new User("Eva", "Ševců", "tsevcu@gmail.com", false);
                        user4.AddKey("eva");
                        user4.Group = basicGroup;


                        dbContextAdd.Groups.Add(leadershipGroup);
                        dbContextAdd.Groups.Add(basicGroup);
                        dbContextAdd.Users.Add(admin);

                        dbContextAdd.Users.Add(user1);
                        dbContextAdd.Users.Add(user2);
                        dbContextAdd.Users.Add(user3);
                        dbContextAdd.Users.Add(user4);

                        dbContextAdd.SaveChanges();
                    }      
                }

                if (dbExists && dbContext.Groups.Where(a => a.Supervisor == null).Any())
                {
                    using (DatabaseContext dbContextAdd = CreateDbContext())
                    {
                        Group leadershipGroup = dbContextAdd.Groups.FirstOrDefault(a => a.Name == "Vedení");
                        Group basicGroup = dbContextAdd.Groups.FirstOrDefault(a => a.Name == "Základní");

                        leadershipGroup.Supervisor = dbContextAdd.Users.FirstOrDefault(a => a.LastName == "admin");
                        basicGroup.Supervisor = dbContextAdd.Users.FirstOrDefault(a => a.LastName == "Ševců" && a.FirstName == "Tomáš");

                        dbContextAdd.SaveChanges();
                    }
                }
            }
        }
    }
}
