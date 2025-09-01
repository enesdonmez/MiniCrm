using Carter;
using FluentValidation;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using MiniCrmApi.DTOs.DealDtos;
using MiniCrmApi.Entities;
using MiniCrmApi.Services.Abstract;

namespace MiniCrmApi.Endpoints;

public class DealEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/deals").RequireRateLimiting("fixed").RequireAuthorization("Admin");

        group.MapGet(string.Empty, async ([FromServices] IDealRepository repository) =>
        { 
            var deals = await repository.GetAllAsync();
            return Results.Ok(deals);
                
        }).WithDescription("list all deal")
            .WithTags("Deals")
            .WithOpenApi();
        
        group.MapPost(string.Empty, async ([FromServices] IDealRepository repository, IMapper mapper, IValidator<Deal> validator , [FromBody] CreateDealDto createDealDto) =>
        { 
            var deal =  mapper.Map<Deal>(createDealDto);
            var validate = await validator.ValidateAsync(deal);
            if (!validate.IsValid) 
            {
                var errors = validate.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray()
                    );
        
                return Results.ValidationProblem(errors);
            }
            await repository.AddAsync(deal); 
            return Results.Created();
                
        }).WithDescription("Add Deal")
            .WithTags("Deals")
            .WithOpenApi();

        group.MapGet("{dealId:guid}", async ([FromServices] IDealRepository repository , [FromRoute] Guid dealId) =>
        { 
            var deal = await repository.GetByIdAsync(dealId);
            if (deal == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(deal);
                
        }).WithDescription("Get Deal by id")
            .WithTags("Deals")
            .WithOpenApi();

        group.MapPut(string.Empty, async ([FromServices] IDealRepository repository , IMapper mapper ,IValidator<Deal> validator , [FromBody] UpdateDealDto updateDealDto) => 
        { 
            var deal =  mapper.Map<Deal>(updateDealDto);
            var validate = await validator.ValidateAsync(deal);
            if (!validate.IsValid) 
            {
                var errors = validate.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray()
                    );
        
                return Results.ValidationProblem(errors);
            }
            var existingDeal = await repository.GetByIdAsync(deal.Id);
            if (existingDeal == null)
                return Results.NotFound();
            await repository.UpdateAsync(deal);
            return Results.Ok();
            
        }).WithDescription("Update Deal")
            .WithTags("Deals")
            .WithOpenApi();


        group.MapDelete ("{id:guid}", async ([FromServices] IDealRepository repository , [FromRoute] Guid Id) =>
        {
            var existingDeal = await repository.GetByIdAsync(Id);
            if (existingDeal == null)
                return Results.NotFound();
            await repository.DeleteAsync(Id);
            return Results.Ok();  
            
        }).WithDescription("Delete Deal")
        .WithTags("Deals")
        .WithOpenApi();

    }
}