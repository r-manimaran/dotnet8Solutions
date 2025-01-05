using CompanyApi.Services;

namespace CompanyApi.Models;

public abstract class AuditableEntity : IAuditable
{
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime ModifiedDate { get; set; }
}