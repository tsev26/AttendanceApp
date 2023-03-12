using System.ComponentModel.DataAnnotations;


namespace Attendance.Domain.Models
{
    public class DomainObject
    {
        [Key]
        public int Id { get; set; }
    }
}
