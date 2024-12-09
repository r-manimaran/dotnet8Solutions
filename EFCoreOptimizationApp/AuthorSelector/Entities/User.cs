using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorSelector.Entities;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; }
    public virtual List<UserRole> UserRoles { get; set; } = new List<UserRole>(); 
    public DateTime Created { get; set; }
    public bool EmailConfirmed { get; set; }
    public DateTime LastActivity {  get; set; }

}
