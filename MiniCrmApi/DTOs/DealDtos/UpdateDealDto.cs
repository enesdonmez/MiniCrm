namespace MiniCrmApi.DTOs.DealDtos;

public record UpdateDealDto
(
    Guid CustomerId,
    string Title,
    decimal Amount,
    string Status
);