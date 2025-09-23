using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public decimal Salary { get; set; }

        [DataType(DataType.Date)]
        public DateTime ContractStart { get; set; }

        [DataType(DataType.Date)]
        public DateTime ContractEnd { get; set; }

        // Зовнішні ключі
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        public int PositionId { get; set; }
        [ForeignKey("PositionId")]
        public Position? Position { get; set; }
    }
}
