using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HRPayroll.Application.Contracts.Identity;
using HRPayroll.Application.DTOs.Auth;
using HRPayroll.Infrastructure.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HRPayroll.Infrastructure.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly JwtOptions _jwtOptions;
    private readonly ConcurrentDictionary<string, RefreshTokenSession> _refreshTokenSessions = new();

    public AuthenticationService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions?.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
    }

    public Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        ValidateCredentials(request.Email, request.Password);

        var user = CreateUser(request.Email);
        var token = CreateAccessToken(user);
        var refreshToken = CreateRefreshToken();

        _refreshTokenSessions[refreshToken] = new RefreshTokenSession(user, token, refreshToken, DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenDays));

        return Task.FromResult(CreateResponse(user, token, refreshToken));
    }

    public Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (!_refreshTokenSessions.TryGetValue(request.RefreshToken, out var session))
        {
            throw new UnauthorizedAccessException("Invalid refresh token.");
        }

        if (!string.Equals(session.AccessToken, request.Token, StringComparison.Ordinal) || session.ExpiresAt <= DateTime.UtcNow)
        {
            _refreshTokenSessions.TryRemove(request.RefreshToken, out _);
            throw new UnauthorizedAccessException("Invalid or expired token pair.");
        }

        _refreshTokenSessions.TryRemove(request.RefreshToken, out _);

        var user = session.User;
        var newAccessToken = CreateAccessToken(user);
        var newRefreshToken = CreateRefreshToken();

        _refreshTokenSessions[newRefreshToken] = new RefreshTokenSession(user, newAccessToken, newRefreshToken, DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenDays));

        return Task.FromResult(CreateResponse(user, newAccessToken, newRefreshToken));
    }

    private static void ValidateCredentials(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            throw new UnauthorizedAccessException("Email and password are required.");
        }
    }

    private AuthUserDto CreateUser(string email)
    {
        var userName = email.Trim();
        var firstName = userName;
        var atIndex = userName.IndexOf('@');

        if (atIndex > 0)
        {
            firstName = userName[..atIndex];
        }

        return new AuthUserDto
        {
            Id = Guid.NewGuid(),
            Email = userName,
            FirstName = firstName,
            LastName = string.Empty,
            Role = "User"
        };
    }

    private string CreateAccessToken(AuthUserDto user)
    {
        if (string.IsNullOrWhiteSpace(_jwtOptions.SigningKey))
        {
            throw new InvalidOperationException("JWT signing key is not configured.");
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Name, string.IsNullOrWhiteSpace(user.FirstName) ? user.Email : user.FirstName),
            new(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expires,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string CreateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    private static AuthResponseDto CreateResponse(AuthUserDto user, string token, string refreshToken)
    {
        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            Expiration = DateTime.UtcNow,
            User = user
        };
    }

    private sealed record RefreshTokenSession(AuthUserDto User, string AccessToken, string RefreshToken, DateTime ExpiresAt);
}