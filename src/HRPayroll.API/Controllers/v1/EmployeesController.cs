using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using HRPayroll.Application.Contracts.Services;
using HRPayroll.Application.DTOs;
using HRPayroll.Application.Models.Common;
using HRPayroll.Application.Contracts.Identity;

namespace HRPayroll.API.Controllers.v1;

/// <summary>
/// Handles HTTP API requests for Employee management.
/// </summary>
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
    }

    /// <summary>
    /// Gets a paginated list of employees with optional search and filtering.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<EmployeeListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployees(
        [FromQuery] string? search,
        [FromQuery] Guid? departmentId,
        [FromQuery] Guid? designationId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _employeeService.GetEmployeesAsync(
            search, departmentId, designationId, page, pageSize, cancellationToken);

        var response = new ApiResponse<IReadOnlyList<EmployeeListDto>>(items, "Employees retrieved successfully.")
        {
            // Custom pagination metadata if desired or wrapper property
        };

        return Ok(response);
    }

    /// <summary>
    /// Gets detailed profile of an employee by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<EmployeeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEmployeeById(Guid id, CancellationToken cancellationToken = default)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id, cancellationToken);
        if (employee == null)
        {
            return NotFound(new ApiResponse<object>(null, $"Employee with ID '{id}' was not found."));
        }

        return Ok(new ApiResponse<EmployeeDto>(employee, "Employee profile retrieved successfully."));
    }

    /// <summary>
    /// Creates a new employee profile.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = $"{RoleNames.SuperAdmin},{RoleNames.HRAdmin},{RoleNames.PayrollManager}")]
    [ProducesResponseType(typeof(ApiResponse<EmployeeDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto createDto, CancellationToken cancellationToken = default)
    {
        var result = await _employeeService.CreateEmployeeAsync(createDto, cancellationToken);
        var response = new ApiResponse<EmployeeDto>(result, "Employee created successfully.");
        return CreatedAtAction(nameof(GetEmployeeById), new { id = result.Id }, response);
    }

    /// <summary>
    /// Updates an existing employee profile.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = $"{RoleNames.SuperAdmin},{RoleNames.HRAdmin},{RoleNames.PayrollManager}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] UpdateEmployeeDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            await _employeeService.UpdateEmployeeAsync(id, updateDto, cancellationToken);
            return NoContent();
        }
        catch (Exception ex) when (ex.Message.Contains("not found"))
        {
            return NotFound(new ApiResponse<object>(null, ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>(null, ex.Message));
        }
    }

    /// <summary>
    /// Deletes (soft deletes) an employee profile.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{RoleNames.SuperAdmin},{RoleNames.HRAdmin}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEmployee(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            await _employeeService.DeleteEmployeeAsync(id, cancellationToken);
            return NoContent();
        }
        catch (Exception ex) when (ex.Message.Contains("not found"))
        {
            return NotFound(new ApiResponse<object>(null, ex.Message));
        }
    }
}
