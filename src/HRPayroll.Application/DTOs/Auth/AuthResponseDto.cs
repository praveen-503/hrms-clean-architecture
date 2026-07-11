namespace HRPayroll.Application.DTOs.Auth;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public DateTime Expiration { get; set; }

    public AuthUserDto User { get; set; } = new();
}