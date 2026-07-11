using HRPayroll.Application.DTOs.Auth;

namespace HRPayroll.Application.Contracts.Identity;

public interface IAuthenticationService
{
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);

    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default);
}