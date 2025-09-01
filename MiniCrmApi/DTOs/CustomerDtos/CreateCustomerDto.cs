namespace MiniCrmApi.DTOs.CustomerDtos;

public record CustomerCreateDto
(
    string FullName,
    string Email,
    string Phone,
    string Company
);