namespace MiniCrmApi.Entities;

public sealed class CustomerNote : BaseEntity
{
    public Guid CustomerId { get; set; }
    public string Note { get; set; } = string.Empty;
    
    public Customer? Customer { get; set; } 

}
