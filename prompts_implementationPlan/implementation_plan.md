# Implementation Plan - Infrastructure Provisioning and Deployment

Provision, configure, secure, connect, and deploy the Enterprise HR & Payroll Management System. Since CLI tools (`az`, `gh`) are not available on this machine, we will utilize the active authenticated browser session with Azure Portal and GitHub to complete the infrastructure setup.

## Proposed Actions

### 1. Azure Resource Provisioning (Via Browser Subagent in Azure Portal)
- Create a Resource Group named `rg-hrms-dev` (Region: East US or nearest available).
- Create a Log Analytics Workspace and an Application Insights resource.
- Create an Azure SQL Logical Server and Azure SQL Database (Pricing tier: Basic, configure firewall to "Allow Azure services and resources to access this server").
- Create an Azure Key Vault:
  - Enable Azure RBAC authorization model.
- Create a Windows App Service Plan (Basic B1) and Azure App Service named `hrms-api-dev`:
  - Runtime stack: .NET 10 (or .NET 9 if .NET 10 is not yet selectable in App Service, and we will update project targets accordingly).
  - Enable System-Assigned Managed Identity.
  - Enable HTTPS Only, WebSockets, HTTP/2.
- Configure Azure Key Vault Secrets:
  - Add secret: `ConnectionStrings--DefaultConnection` (value: Azure SQL connection string).
  - Add secrets for JWT settings, Refresh Token, SMTP, and Application URL.
- Grant App Service Managed Identity the "Key Vault Secrets User" role.
- Create Azure Static Web App named `hrms-portal-dev`:
  - Connect to the GitHub repository, select main branch, set build preset to Angular.

### 2. Backend Code Updates (Local Workspace)
- Update [Program.cs](file:///d:/Praveen_Github_Projects/hrms-clean-architecture/src/HRPayroll.API/Program.cs) to integrate Azure Key Vault:
  - Use `DefaultAzureCredential` to load configuration from Azure Key Vault when running in production.
  - Configure Application Insights telemetry.
- Update [appsettings.json](file:///d:/Praveen_Github_Projects/hrms-clean-architecture/src/HRPayroll.API/appsettings.json) and production-specific configurations.

### 3. GitHub Actions Workflows
- Create [backend.yml](file:///d:/Praveen_Github_Projects/hrms-clean-architecture/.github/workflows/backend.yml):
  - Triggered on push to `main` branch under `src/` prefix.
  - Restores, builds, runs tests, publishes, and deploys to Azure App Service using Azure Login/Publish Profile.
- Create [frontend.yml](file:///d:/Praveen_Github_Projects/hrms-clean-architecture/.github/workflows/frontend.yml) or let SWA configuration manage the frontend deployment workflow file.

### 4. Verification & Validation
- Verify Azure SQL Database connectivity.
- Verify App Service reads secrets successfully from Key Vault.
- Verify Static Web App loads and talks to the App Service.

## Open Questions
- **Pricing Confirmation**: The pricing tiers requested are Basic B1 for App Service Plan and Basic for SQL Database. Please approve to proceed with these tiers.
- **Azure Region**: We plan to use `East US` as the primary region. Please let us know if another region is preferred.
- **GitHub Credentials**: We will configure GitHub repository secrets via browser interaction since `gh` CLI is not available. Please confirm this is acceptable.
