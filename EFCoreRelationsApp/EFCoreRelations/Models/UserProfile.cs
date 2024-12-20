namespace EFCoreRelations.Models;

public class UserProfile
{
    public int Id { get; set; }
    public string Bio { get; set; }
    
    //Foreign Key to User Table
    public int UserId { get; set; }
    //Navigation Property for User
    public User User { get; set; }
}
