namespace MiniCrmApi.DTOs.DealDtos;

public record CreateDealDto
(
    Guid CustomerId,
    string Title,
    decimal Amount,
    string Status
);