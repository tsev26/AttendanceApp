using System.ComponentModel.DataAnnotations;


namespace Attendance.Domain.Models
{
    public class DomainObject
    {
        private static int _nextID = 1;
        public DomainObject()
        {
            ID = _nextID++;
        }


        [Key]
        public int ID { get; set; }
    }
}
