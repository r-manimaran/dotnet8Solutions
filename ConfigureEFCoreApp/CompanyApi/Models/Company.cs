namespace CompanyApi.Models;

public class Company : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; }  
    
    // Navigation Properties
    public ICollection<User> Users { get; set; }
    public ICollection<Project> Projects { get; set; }
}
