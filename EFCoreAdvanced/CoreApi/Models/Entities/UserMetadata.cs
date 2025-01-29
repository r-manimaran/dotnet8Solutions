namespace CoreApi.Models.Entities;

public class UserMetadata
{
    public DateTime AccountCreated { get; set; } 
    public string CreatedBy { get; set; }
    public List<string> Tags { get; set; }
}
