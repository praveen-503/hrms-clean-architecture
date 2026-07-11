using System.Collections.Generic;

namespace HRPayroll.Application.Models.Common;

/// <summary>
/// A standardized wrapper for all API responses.
/// </summary>
/// <typeparam name="T">The type of the response data payload.</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Indicates whether the API call succeeded.
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// Optional status or descriptive message.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// List of errors if the operation failed.
    /// </summary>
    public List<string>? Errors { get; set; }

    /// <summary>
    /// The actual payload of the response.
    /// </summary>
    public T? Data { get; set; }

    public ApiResponse()
    {
    }

    public ApiResponse(T data, string? message = null)
    {
        Succeeded = true;
        Message = message;
        Data = data;
    }

    public ApiResponse(string message, List<string>? errors = null)
    {
        Succeeded = false;
        Message = message;
        Errors = errors;
    }

    public static ApiResponse<T> Success(T data, string? message = null) => new(data, message);
    public static ApiResponse<T> Failure(string message, List<string>? errors = null) => new(message, errors);
}
