namespace CoreApi.Models.Entities;

public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal Total { get; set; }
    public bool IsDeleted { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }
}
