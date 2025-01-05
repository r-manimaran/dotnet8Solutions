namespace CompanyApi.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public int CompanyId { get; set; } //Foreign Key for company

    // Navigation properties
    public Company Company { get; set; }
    public ICollection<UserProject> UserProjects { get; set; }


}
