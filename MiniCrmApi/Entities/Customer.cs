namespace MiniCrmApi.Entities;

public sealed class Customer : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    
    public bool IsDeleted { get; set; } = false;

    public List<Deal> Deals { get; set; } = new();
    public List<CustomerNote> Notes { get; set; } = new();
}
