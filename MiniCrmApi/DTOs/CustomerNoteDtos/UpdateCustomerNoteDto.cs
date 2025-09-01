namespace MiniCrmApi.DTOs.CustomerNoteDtos;

public record UpdateCustomerNoteDto
(
    Guid Id,
    Guid CustomerId,
    string Note
);