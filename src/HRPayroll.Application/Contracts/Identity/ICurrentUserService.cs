namespace HRPayroll.Application.Contracts.Identity;

/// <summary>
/// Service contract to retrieve details about the current authenticated user.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the unique identifier or username of the current user.
    /// </summary>
    string? UserId { get; }
}
