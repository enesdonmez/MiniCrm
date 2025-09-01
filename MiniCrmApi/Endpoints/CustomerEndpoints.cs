using Carter;
using FluentValidation;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MiniCrmApi.DTOs.CustomerDtos;
using MiniCrmApi.DTOs.EmailDtos;
using MiniCrmApi.Entities;
using MiniCrmApi.Services.Abstract;

namespace MiniCrmApi.Endpoints;

public class CustomerEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/customers").RequireRateLimiting("fixed").RequireAuthorization("Admin"); 
        
        group.MapGet(string.Empty, async (ICustomerRepository repository) =>
        {
            var customers = await repository.GetAllAsync();
            return Results.Ok(customers);
            

        }).WithDescription("list all customer")
        .WithTags("Customers")
        .WithOpenApi();
        
        group.MapPost(string.Empty, async (ICustomerRepository repository, IMapper mapper , IValidator<Customer> validator ,[FromBody] CustomerCreateDto customerCreateDto) =>
        {
            var customer = mapper.Map<Customer>(customerCreateDto);
            var validateResult = await validator.ValidateAsync(customer);
            if (!validateResult.IsValid) 
            {
                var errors = validateResult.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray()
                    );
        
                return Results.ValidationProblem(errors,statusCode: StatusCodes.Status400BadRequest);
            }
            await repository.AddAsync(customer);
            return Results.Created();
            
        }).WithDescription("Add customer")
        .WithTags("Customers")
        .WithOpenApi();

        group.MapGet("{customerId:guid}", async (ICustomerRepository repository, [FromRoute] Guid customerId) =>
        {
            var customer = await repository.GetByIdAsync(customerId);
            if (customer == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(customer);

        }).WithDescription("Get customer by id")
        .WithTags("Customers")
        .WithOpenApi();

        group.MapPut(string.Empty, async (ICustomerRepository repository ,IMapper mapper , IValidator<Customer> validator, [FromBody] UpdateCustomerDto customerUpdateDto) =>
        {
            var customer = mapper.Map<Customer>(customerUpdateDto);
            var validateResult = await validator.ValidateAsync(customer);
            if (!validateResult.IsValid) 
            {
                var errors = validateResult.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray()
                    );
        
                return Results.ValidationProblem(errors,statusCode: StatusCodes.Status400BadRequest);
            }
            await repository.UpdateAsync(customer);
            return Results.Ok();
            
        }).WithDescription("Update customer")
        .WithTags("Customers")
        .WithOpenApi();

        group.MapDelete(string.Empty, async (ICustomerRepository repository , [FromQuery] Guid customerId) =>
        {
            await repository.DeleteAsync(customerId);
            return Results.Ok();
            
        }).WithDescription("Delete customer")
        .WithTags("Customers")
        .WithOpenApi();

        group.MapPost("/send-email", async (EmailRequestDto request, IEmailService emailService) =>
        {
            await emailService.SendEmailAsync(request.To, request.Subject, request.Body);
            return Results.Ok("Email sent.");

        }).WithDescription("Sending Email")
        .WithTags("Customers")
        .WithOpenApi(); 
    }
}