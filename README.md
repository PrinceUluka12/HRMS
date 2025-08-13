# HRMS API Documentation

## Overview

The HRMS (Human Resource Management System) API is a robust ASP.NET Core application built with Clean Architecture principles. It provides comprehensive HR functionality including employee management, department organization, equipment tracking, and buddy pairing for onboarding.

## Key Features

- **Employee Management**: Full CRUD operations for employee records with rich personal, contact, and employment details
- **Department Management**: Create and manage organizational departments with role-based access control
- **Equipment Tracking**: Manage inventory, assignments, and returns of company equipment
- **Buddy Pairing**: Facilitate onboarding and mentorship through structured pairing
- **Real-time Notifications**: SignalR integration for WebSocket-based event notifications
- **Health Monitoring**: Built-in health checks for system dependencies

## Technical Stack

- **Framework**: ASP.NET Core
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: Azure AD with JWT Bearer tokens
- **Architecture**: Clean Architecture with CQRS pattern via MediatR
- **API Documentation**: Swagger/OpenAPI

## Getting Started

### Prerequisites

- .NET 9 SDK
- SQL Server instance
- Azure AD configuration (for authentication)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/PrinceUluka12/HRMS.git
   cd HRMS
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Configure the application:
   - Copy `HRMS.API/appsettings.Development.json` to `appsettings.json`
   - Update connection strings and authentication settings

4. Apply database migrations:
   ```bash
   cd HRMS.Infrastructure
   dotnet ef database update --context HRMSDbContext
   ```

5. Run the application:
   ```bash
   dotnet run --project HRMS.API
   ```

The API will be available at `https://localhost:5001` with Swagger UI at `/swagger`.

## API Endpoints

### Employees

| Method | Endpoint | Description | Authorization |
|--------|----------|-------------|---------------|
| POST | `/api/employees` | Create new employee | Admin |
| GET | `/api/employees/{id}` | Get employee by ID | Any authenticated |
| GET | `/api/employees` | List employees with filtering | Any authenticated |
| GET | `/api/employees/azure` | Get employee by Azure AD ID | Any authenticated |
| PUT | `/api/employees/{id}` | Update employee | Admin |
| DELETE | `/api/employees/{id}` | Delete employee | Admin |

### Departments

| Method | Endpoint | Description | Authorization |
|--------|----------|-------------|---------------|
| GET | `/api/departments` | List all departments | Employee |
| POST | `/api/departments` | Create new department | Admin |
| PUT | `/api/departments/{id}` | Update department | Admin |
| DELETE | `/api/departments/{id}` | Delete department | Admin |

### Equipment

| Method | Endpoint | Description | Authorization |
|--------|----------|-------------|---------------|
| POST | `/api/equipments/AssignEquipment` | Assign equipment to employee | Authorized roles |
| POST | `/api/equipments/ReturnEquipment` | Return equipment to inventory | Authorized roles |
| GET | `/api/equipments` | List all equipment | Any authenticated |
| GET | `/api/equipments/assignments/active` | List active assignments | Any authenticated |

### Buddy Pairs

| Method | Endpoint | Description | Authorization |
|--------|----------|-------------|---------------|
| POST | `/api/buddypair/create` | Create new buddy pair | Authorized roles |
| GET | `/api/buddypair/employee` | Get pair by employee ID | Any authenticated |

### Account

| Method | Endpoint | Description | Authorization |
|--------|----------|-------------|---------------|
| GET | `/api/account/profile` | Get authenticated user's profile | Authenticated |

### Health

| Method | Endpoint | Description | Authorization |
|--------|----------|-------------|---------------|
| GET | `/api/health` | Basic health check | Public |
| GET | `/healthz` | Detailed health status | Public |

## Authentication

The API uses Azure AD for authentication. To authenticate:

1. Obtain a JWT token from your Azure AD tenant
2. Include the token in the `Authorization` header as a Bearer token:
   ```
   Authorization: Bearer <your_token>
   ```

## Error Handling

The API returns standardized error responses following RFC 7807 (Problem Details). Common HTTP status codes include:

- 400 Bad Request: Validation errors or invalid input
- 401 Unauthorized: Missing or invalid authentication
- 403 Forbidden: Authenticated but not authorized
- 404 Not Found: Resource doesn't exist
- 500 Internal Server Error: Unexpected server error



For detailed endpoint documentation, please refer to the Swagger UI at `/swagger` when running the application locally.
