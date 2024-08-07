﻿using WhispMe.BLL.AuthInterfaces;
using WhispMe.DAL.Entities;
using WhispMe.DAL.Interfaces;
using WhispMe.DTO.DTOs;
using WhispMe.DTO.Exceptions;

namespace WhispMe.BLL.AuthServices;

public class AuthService : IAuthService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IJwtTokenGenerator jwtTokenGenerator, IUnitOfWork unitOfWork)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> RegisterAsync(RegisterRequestDto registerRequestDto)
    {
        try
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerRequestDto.Password + salt);

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                FullName = registerRequestDto.FullName.Trim(),
                Email = registerRequestDto.Email.Trim(),
                PasswordHash = passwordHash.Trim(),
                Salt = salt,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.UserRepository.CreateAsync(user);
            return user.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred during registration.", ex);
        }
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(loginRequestDto.Email)
                ?? throw new UnauthorizedAccessException("Invalid email or password");

            if (!BCrypt.Net.BCrypt.Verify(loginRequestDto.Password + user.Salt, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            return new LoginResponseDto
            {
                Token = _jwtTokenGenerator.GenerateToken(user, user.Roles),
                User = new UserDto
                {
                    FullName = user.FullName,
                    Email = user.Email,
                },
            };
        }
        catch (Exception ex) when (ex is NotFoundException || ex is UnauthorizedAccessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred during login.", ex);
        }
    }
}
