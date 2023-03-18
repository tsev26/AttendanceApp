using System.ComponentModel.DataAnnotations;


namespace Attendance.Domain.Models
{
    public class DomainObject
    {
        private static int _nextId = 1;
        public DomainObject()
        {
            Id = _nextId++;
        }


        [Key]
        public int Id { get; set; }
    }
}
