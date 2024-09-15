using Microsoft.EntityFrameworkCore.Update.Internal;

namespace cartesionExplosion_api.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
        public string Skills { get; set; }
        public int YearsOfExperience { get; set; }
        public bool IsManager { get; set; }
        public int TeamId { get; set; }
        public List<Task> Tasks { get; set; } = new();
        public List<SalaryPayment> SalaryPayments { get; set; } = new();
    }
}
