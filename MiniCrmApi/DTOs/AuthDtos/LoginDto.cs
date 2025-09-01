namespace MiniCrmApi.DTOs.AuthDtos;

public sealed record LoginDto
(
    string Email,
    string Password
);