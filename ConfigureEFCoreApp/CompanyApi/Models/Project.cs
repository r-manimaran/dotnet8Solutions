namespace CompanyApi.Models;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public int CompanyId { get; set; } // Foreign Key for Company

    // Navigation Properties
    public Company Company { get; set; }
    public ICollection<UserProject> UserProjects { get; set; }
}
