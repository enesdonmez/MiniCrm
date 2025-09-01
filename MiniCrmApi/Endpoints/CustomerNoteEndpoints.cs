using Carter;
using FluentValidation;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using MiniCrmApi.DTOs.CustomerNoteDtos;
using MiniCrmApi.Entities;
using MiniCrmApi.Services.Abstract;

namespace MiniCrmApi.Endpoints;

public class CustomerNoteEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/customer-notes").RequireRateLimiting("fixed").RequireAuthorization("Admin");

        group.MapGet(string.Empty, async (ICustomerNoteRepository customerNoteRepository) =>
        {
            var notes = await customerNoteRepository.GetAllAsync();
            return Results.Ok(notes);
            
        }).WithDescription("Get All CustomerNotes").WithTags("CustomerNotes").WithOpenApi();

        group.MapPost(string.Empty, async (ICustomerNoteRepository customerNoteRepository , IMapper mapper , IValidator<CustomerNote> validate ,[FromBody] CreateCustomerNoteDto customerNoteDto) =>
        {
            var customerNote =  mapper.Map<CustomerNote>(customerNoteDto);
            var validationResult = await validate.ValidateAsync(customerNote);
            if (!validationResult.IsValid) 
            {
                var errors = validationResult.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray()
                    );
        
                return Results.ValidationProblem(errors,statusCode: StatusCodes.Status400BadRequest);
            }
            await customerNoteRepository.AddAsync(customerNote);
            return Results.Created();
            
        }).WithDescription("Get All CustomerNotes").WithTags("CustomerNotes").WithOpenApi();

        group.MapGet("{customerNoteId:guid}", async (ICustomerNoteRepository customerNoteRepository , [FromRoute] Guid customerNoteId) =>
        {
            var note = await customerNoteRepository.GetByIdAsync(customerNoteId);
            if (note == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(note);
            
        }).WithDescription("Get All CustomerNotes")
        .WithTags("CustomerNotes")
        .WithOpenApi();

        group.MapPut(string.Empty, async (ICustomerNoteRepository  customerNoteRepository , IMapper mapper, IValidator<CustomerNote> validate ,[FromBody] UpdateCustomerNoteDto customerNoteDto) =>
        {
            var customerNote =  mapper.Map<CustomerNote>(customerNoteDto);
            var validationResult = await validate.ValidateAsync(customerNote);
            if (!validationResult.IsValid) 
            {
                var errors = validationResult.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray()
                    );
        
                return Results.ValidationProblem(errors,statusCode: StatusCodes.Status400BadRequest);
            }
            await customerNoteRepository.UpdateAsync(customerNote);
            return Results.Ok();
            
        }).WithDescription("Get All CustomerNotes").WithTags("CustomerNotes").WithOpenApi();

        group.MapDelete(string.Empty, async (ICustomerNoteRepository  customerNoteRepository , Guid customerNoteId) =>
        {
            await customerNoteRepository.DeleteAsync(customerNoteId);
            return Results.Ok();
            
        }).WithDescription("Get All CustomerNotes").WithTags("CustomerNotes").WithOpenApi();

    }
}