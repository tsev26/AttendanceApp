using Attendance.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.EF
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityGlobalSetting> ActivityGlobalSetting { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<AttendanceRecordFix> AttendanceRecordFixes { get; set; }
        public DbSet<AttendanceTotal> AttendanceTotals { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Key> Keys { get; set; }
        public DbSet<ActivityProperty> ActivityProperties { get; set; }
        public DbSet<Obligation> Obligations { get; set; }
    }
}
