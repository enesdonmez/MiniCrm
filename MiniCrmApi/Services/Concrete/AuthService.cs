using Microsoft.AspNetCore.Identity;
using MiniCrmApi.Auth;
using MiniCrmApi.DTOs.AuthDtos;
using MiniCrmApi.DTOs.TokenDtos;
using MiniCrmApi.Entities;
using MiniCrmApi.Services.Abstract;

namespace MiniCrmApi.Services.Concrete;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly JwtGenerator _tokenGen;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(IUserRepository userRepo, JwtGenerator tokenGen, IPasswordHasher<User> passwordHasher)
    {
        _userRepo = userRepo;
        _tokenGen = tokenGen;
        _passwordHasher = passwordHasher;
    }

    public async Task<string?> RegisterAsync(RegisterDto dto)
    {
        var existing = await _userRepo.GetByEmailAsync(dto.Email);
        if (existing != null) return null;

        var user = new User
        {
            UserName = dto.Email,
            Email = dto.Email,
            PasswordHash = _passwordHasher.HashPassword(null!, dto.Password)
        };

        TokenDto tokenDto = new TokenDto
        (
            user.Id, 
            user.Email,
            "admin"
        );

        await _userRepo.AddAsync(user);
        return "User Created";
    }

    public async Task<string?> LoginAsync(LoginDto dto)
    {
        var user = await _userRepo.GetByEmailAsync(dto.Email);
        if (user == null) return null;

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (result == PasswordVerificationResult.Failed) return null;
        TokenDto tokenDto = new TokenDto(
            user.Id, 
            user.Email,
            "admin"
        );
        return _tokenGen.GenerateToken(tokenDto);
    }
}