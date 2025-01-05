namespace CompanyApi.Models;

public class UserProject
{
    public int UserId { get; set; }
    public User User { get; set; }
    
    public int ProjectId { get; set; }
    public Project Project { get; set; }

    public DateTime AssignedDate { get; set; }
    public string Role { get; set; }
}
