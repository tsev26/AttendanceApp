using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Models
{
    public class Activity : DomainObject
    {
        public Activity(string name, string shortcut, ActivityProperty property) : base()
        {
            Name = name;
            Shortcut = shortcut;
            Property = property;
        }

        public Activity(Activity activity) : base()
        {
            Id = activity.Id;
            Name = activity.Name;
            Shortcut = activity.Shortcut;
            Property = activity.Property.Clone();
        }

        public string Name { get; set; }
        public string Shortcut { get; set; }
        public ActivityProperty Property { get; set; }



        private int PositionOfShortCutInName => Name.ToLower().IndexOf(Shortcut.ToLower()) + 1;
        private int LenghtOfName => Name.Length;

        public string ActName1 => (PositionOfShortCutInName != 0) ? Name.Substring(0, PositionOfShortCutInName - 1) : Name;
        public string ActName2 => Shortcut;
        public string ActName3 => (PositionOfShortCutInName != 0) ? Name.Substring(PositionOfShortCutInName, LenghtOfName - PositionOfShortCutInName) : "";

        public Activity Clone()
        {
            return new Activity(this)
            {
                Id = this.Id,
                Property = Property?.Clone()
            };
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
