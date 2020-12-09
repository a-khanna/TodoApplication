# Todo API

A .Net Core API for managing task lists. Lists and items within the list can be assigned labels for categorization.
Supports REST and GraphQL endpoints.

### Techologies
.Net Core 3.1, EF Core, GraphQL (including playground), Swagger (documentation)

### Database Setup
Pass SQL Server connection string in the appsettings.
`"ConnectionStrings": { "TodoConnectionString": <your connection string> }`

Set the below flag in case seeding is needed. This will also ensure database creation at the application start.
`"SeedDatabase":  true`

Database migrations are also available.
It is recommended to first run the migration and then use seeding.

### Logging
Uses NLog as the logging provider.
Logs are stored in logs.txt in the executing directory.
Logs Correlation-Id in all the logs within the request pipeline.
Logs all request and response details (including message body).
Format for log messages is: datetime  correlation-id  log-level  class-name  log-message  exception