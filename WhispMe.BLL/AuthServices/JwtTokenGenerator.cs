using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WhispMe.BLL.AuthInterfaces;
using WhispMe.DAL.Entities;
using WhispMe.DAL.Interfaces;
using WhispMe.DTO;

namespace WhispMe.BLL.AuthServices;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtOptions _jwtOptions;
    private readonly IUnitOfWork _unitOfWork;
    // private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions, IUnitOfWork unitOfWork)
    {
        _jwtOptions = jwtOptions.Value;
        _unitOfWork = unitOfWork;
        // _httpContextAccessor = httpContextAccessor;  IHttpContextAccessor httpContextAccessor,
    }

    public string GenerateToken(User user, IEnumerable<string> roles)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

        var claimList = new List<Claim>
        {
            new("userId", user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Sub, user.Email),
            new(JwtRegisteredClaimNames.GivenName, user.FullName),
        };

        // TODO: Add role in token

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _jwtOptions.Audience,
            Issuer = _jwtOptions.Issuer,
            Subject = new ClaimsIdentity(claimList),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Adding a cookie in ASP.NET Core
        //var authCookieOptions = new CookieOptions
        //{
        //    HttpOnly = true,
        //    Expires = DateTime.UtcNow.AddDays(7),
        //};

        //_httpContextAccessor.HttpContext.Response.Cookies.Append("userId", user.Id, authCookieOptions);
        //var rolesString = string.Join(",", roles);
        //_httpContextAccessor.HttpContext.Response.Cookies.Append("roles", rolesString, authCookieOptions);

        return tokenHandler.WriteToken(token);
    }
}
