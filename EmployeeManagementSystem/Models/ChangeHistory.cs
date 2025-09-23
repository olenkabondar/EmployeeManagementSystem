namespace EmployeeManagementSystem.Models
{
    public class ChangeHistory
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public string FieldName { get; set; } = string.Empty;
        public string OldValue { get; set; } = string.Empty;
        public string NewValue { get; set; } = string.Empty;

        public DateTime ChangedAt { get; set; } = DateTime.Now;

        public string ChangedBy { get; set; } = string.Empty;
    }
}
