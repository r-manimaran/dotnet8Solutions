namespace EFCoreRelations.Models;
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    //Navigation Property
    public UserProfile Profile { get; set; }
}
