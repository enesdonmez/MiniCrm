namespace MiniCrmApi.DTOs.CustomerNoteDtos;

public record CreateCustomerNoteDto
(
    Guid CustomerId,
    string Note
);