namespace MiniCrmApi.DTOs.CustomerDtos;

public record UpdateCustomerDto
(
    string FullName,
    string Email,
    string Phone,
    string Company
);