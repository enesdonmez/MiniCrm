namespace MiniCrmApi.Entities;

public sealed class Deal : BaseEntity
{
    public Guid CustomerId { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = "Pending";
    
    public Customer? Customer { get; set; } 
}
