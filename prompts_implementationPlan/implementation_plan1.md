# Implementation Plan: Enterprise HR & Payroll Management System (Phase 1)

This plan outlines the setup of the production-ready Enterprise HR & Payroll Management System using .NET 10 (Clean Architecture) and Angular 20. 

In Phase 1, we will lay down the foundational infrastructure, shared library structures, API setup, database context, and then implement the core **Employee Module** following the strict incremental development rules requested.

---

## User Review Required

> [!IMPORTANT]
> - We will use **Entity Framework Core Code First** with SQL Server. Please make sure you have SQL Server running locally or update the connection string in `appsettings.json` once generated.
> - We will implement **Soft Delete**, **Audited Entities**, and **Optimistic Concurrency** as part of the core infrastructure.
> - We will use .NET 10 features like C# 14 file-scoped namespaces, primary constructors where appropriate, and clean type mappings.

---

## Open Questions

> [!NOTE]
> 1. Do you have a specific database name or SQL Server local instance name (e.g. `(localdb)\MSSQLLocalDB` or `.` or `localhost`) you prefer for the default connection string? We will default to `Server=(local);Database=HRPayrollDb;Trusted_Connection=True;TrustServerCertificate=True;` if unspecified.

---

## Proposed Changes

We will create the solution structure and projects under `src/` and tests under `tests/`.

### 1. Solution & Projects Setup

We will create the following projects and link them:
*   `HRPayroll.Domain`: Entities, Enums, Value Objects, Specifications, Interfaces.
*   `HRPayroll.Application`: DTOs, Services, Interfaces, Validators (FluentValidation), Mapping Profiles (AutoMapper), Exceptions, Behaviors.
*   `HRPayroll.Infrastructure`: External services (Email, PDF/Excel generation, JWT Provider).
*   `HRPayroll.Persistence`: DbContext, Entity Configurations, Generic & Specific Repositories, Unit of Work, Migrations, Seed Data.
*   `HRPayroll.Shared` & `HRPayroll.Common`: Shared contracts, Constants, Utility classes.
*   `HRPayroll.API`: Controllers, Middleware, Swashbuckle/OpenAPI setup, Program.cs.

### 2. Core Domain Abstractions (`HRPayroll.Domain` & `HRPayroll.Shared`)
*   `[NEW]` `IBaseEntity` / `BaseEntity` with `Id`, `CreatedBy`, `CreatedOn`, `LastModifiedBy`, `LastModifiedOn`, `IsDeleted`, `DeletedBy`, `DeletedOn` properties.
*   `[NEW]` `ISoftDelete` interface to support automatic filtering in EF Core.
*   `[NEW]` `IAuditable` interface.
*   `[NEW]` Concurrency token property `RowVersion` (byte array) on base/selected entities.

### 3. Persistence Foundation (`HRPayroll.Persistence`)
*   `[NEW]` `HRPayrollDbContext`: Configures DbContext, automatically applies configurations from assembly, overrides `SaveChangesAsync` to auto-populate `IAuditable` properties and handle soft deletes.
*   `[NEW]` `GenericRepository<T>` and `IGenericRepository<T>`.
*   `[NEW]` `UnitOfWork` and `IUnitOfWork`.

### 4. Application Framework Setup (`HRPayroll.Application` & `HRPayroll.API`)
*   `[NEW]` Global Exception Handling Middleware returning JSON `ProblemDetails` formatted API Responses.
*   `[NEW]` `ApiResponse<T>` wrappers for consistent API responses.
*   `[NEW]` API Versioning support.

### 5. Employee Module Implementation (Incremental Step-by-Step)
*   **Database Design/Entities**: `Employee` entity with owned type `Address` (Value Object), plus relationships to `Department` and `Designation`.
*   **Configurations**: EF Core Entity Type Configuration for `Employee` including indexes, unique keys, soft delete query filters.
*   **Repositories**: `IEmployeeRepository` / `EmployeeRepository`.
*   **DTOs**: `EmployeeDto`, `CreateEmployeeDto`, `UpdateEmployeeDto`, `EmployeeListDto`.
*   **Validators**: FluentValidation rules for creation/updates.
*   **Services & Interfaces**: `IEmployeeService` / `EmployeeService` coordinating transactions via Unit of Work and implementing business validations.
*   **AutoMapper**: `EmployeeMappingProfile`.
*   **Controller**: `EmployeesController` with API versioning, Pagination, Filtering, Sorting, and Swagger docs.

---

## Verification Plan

### Automated Tests
*   Run dotnet build to ensure compilation is successful.
*   Run basic entity configurations verification.

### Manual Verification
*   Verify Swagger UI is loading and endpoints are documented with parameters (filtering, pagination).
*   Test DbContext SaveChanges behavior with audit logging and soft delete via a test endpoint.
