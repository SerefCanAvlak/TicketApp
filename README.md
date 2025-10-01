# Ticketing System

A modular, event-driven ticketing system built with **ASP.NET Core**, **Entity Framework Core**, **MediatR**, and modern DDD practices. The solution supports event management, seat reservations, ticket sales, notifications, and user authentication/authorization.

---

## Features

- **Event Management:** Create, update, delete, and list events.
- **Seat Management:** Manage seat layouts, query free/occupied seats.
- **Ticketing:** Reserve and purchase tickets, lock seats, and manage ticket states.
- **User Management:** Register, update, delete users; admin and regular user roles.
- **Authentication:** JWT-based login, registration, and refresh tokens and Keycloak based OAuth 2.0.
- **Notifications:** Email notifications for ticketing events (in-memory, event-driven, or RabbitMQ).
- **Logging:** Centralized logging with log4net and log querying endpoints.
- **Background Jobs:** Worker service for processing notifications and event expiration.

---

## Solution Structure

```
Ticketing.sln
Ticketing/
  Ticketing.Application/    # Application layer (CQRS, MediatR, DTOs, validation)
  Ticketing.Domain/         # Domain entities, enums, interfaces
  Ticketing.Infrastructure/ # EF Core, repositories, services, migrations
  Ticketing.WebAPI/         # ASP.NET Core Web API
Ticketing.Worker/           # Background worker for jobs (notifications, event expiration)
```

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)
- [MailDev](https://maildev.github.io/maildev/) or SMTP server for email testing

### Configuration

#### Database

Update the connection string in `Ticketing/appsettings.json` and `Ticketing.Worker/appsettings.json`:

```json
"ConnectionStrings": {
  "SqlServer": "Server=localhost;Database=TicketingDb;User Id=sa;Password=your_password;"
}
```

#### JWT

Configure JWT settings in `appsettings.json` under the `Jwt` section.

#### Email

The default configuration uses SMTP on `localhost:2525` (MailDev). Adjust as needed.

---

### Database Migration

Run the following commands to create and update the database:

```sh
cd Ticketing
dotnet ef database update --startup-project ../Ticketing.WebAPI
```

---

### Running the Application

#### Web API

```sh
cd Ticketing/Ticketing.WebAPI
dotnet run
```

#### Worker Service

```sh
cd Ticketing.Worker
dotnet run
```

---

## Usage

- **API Endpoints:**  
  The Web API exposes endpoints for events, tickets, seats, users, and authentication.  
  See `Ticketing.WebAPI/Controllers` for details.

- **Admin Access:**  
  The first user with username `admin` is created automatically on startup if not present.

- **Background Jobs:**  
  The worker processes notifications and event expiration in the background.

---

## Technologies Used

- **ASP.NET Core 8**
- **Entity Framework Core**
- **MediatR (CQRS)**
- **AutoMapper**
- **FluentValidation**
- **log4net**
- **FluentEmail**
- **Dapper** (for log querying)
- **Microsoft Identity**
- **Keycloak**

---

## Extending

- Add new features by creating CQRS command/query handlers in `Ticketing.Application`.
- Add new background jobs by implementing `IWorkerJob`.
- Add new domain logic in `Ticketing.Domain`.

---

## License

This project is proprietary and for demonstration purposes only.

---

## Authors

- [Şeref Can Avlak]

---

## Related Files & References

- `Ticketing.sln`
- `Ticketing.Application/Features`
- `Ticketing.Domain/Entities`
- `Ticketing.Infrastructure/Context/ApplicationDbContext.cs`
- `Ticketing.WebAPI/Controllers`
- `Ticketing.Worker/Services/WorkerService.cs`

---

For more details, see the code and comments in each project folder.
