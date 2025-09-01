using Carter;
using Microsoft.AspNetCore.Mvc;
using MiniCrmApi.DTOs.AuthDtos;
using MiniCrmApi.Services.Abstract;

namespace MiniCrmApi.Endpoints;

public class AuthEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/auth").WithTags("Auth");

        group.MapPost("/register", async (RegisterDto dto, [FromServices] IAuthService service) =>
        {
            var token = await service.RegisterAsync(dto);
            return token is null ? Results.BadRequest("Email already exists") : Results.Ok(new { token });
        });

        group.MapPost("/login", async (LoginDto dto, [FromServices] IAuthService service) =>
        {
            var token = await service.LoginAsync(dto);
            return token is null ? Results.Unauthorized() : Results.Ok(new { token });
        }); 
    }
}