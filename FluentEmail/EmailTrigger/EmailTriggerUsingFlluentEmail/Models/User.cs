namespace EmailTriggerUsingFlluentEmail.Models
{
    public class User
    {
        public User(string name, string email, string? memberType)
        {
            Name = name;
            Email = email;
            MemberType = memberType; // "Basic" or "Premium" or null for non-members
        }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? MemberType { get; set; } // null for non-members, "Basic" or "Premium" for members
    }
}
