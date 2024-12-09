using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorSelector.Entities;

public class Role
{
    public int Id { get; set; }
    public virtual List<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public string Name { get; set; }
}
