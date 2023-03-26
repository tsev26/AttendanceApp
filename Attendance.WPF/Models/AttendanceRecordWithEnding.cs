﻿using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.WPF.Model
{
    public class AttendanceRecordWithEnding
    {
        public AttendanceRecordWithEnding(User user, Activity activity, DateTime entry, DateTime exit)
        {
            User = user;
            Activity = activity;
            Entry = entry;
            Exit = exit;
        }

        public User User { get; set; }
        public Activity Activity { get; set; }
        public DateTime Entry { get; set; }
        public DateTime Exit { get; set; }
    }
}
