using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class Position
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
