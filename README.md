# Todo API

A .Net Core API for managing task lists. Lists and items within the list can be assigned labels for categorization.
Supports REST and GraphQL endpoints.

### Techologies
.Net Core 3.1, EF Core, GraphQL (including playground), Swagger documentation

### Database Setup
Pass the SQL Server connection string in the appsettings.

    "ConnectionStrings":
	{
		"TodoConnectionString": <your connection string>
	}

Set `"SeedDatabase":  true` in case seeding is needed. This will also ensure database creation at the application start.

Database migrations are also available.
It is recommended to first run the migration and then use seeding.

### Logging
Uses NLog as the logging provider.
Logs are stored in logs.txt in the executing directory.
Logs Correlation-Id along with all the logs within the request pipeline.
Logs all request and response details (including message body).

Format for the log messages is: 

    datetime	correlation-id	log-level	class-name	log-message	exception(if any)

### Running the API
Navigate to the solution in `cmd` and run `dotnet restore`.
Navigate to the API project in `cmd` and run `dotnet run`.

The APIs are accessible only by an authenticated user.

##### Swagger
Open `localhost:5000` in a browser to view the Swagger documentation.

To login, fire a request using `/api/v1/todo/User/authenticate` endpoint from the swagger UI. The request body is pre-filled with the username and password from seeding.

Copy the JWT from the `model` key inside the response.
Click on the `Authorize` button on the swagger UI and paste the token in `Bearer <copied token>` format.

##### GraphQL
GraphQL playground can be accessed on `localhost:5000\playground`.
Use the below mutation to get the JWT.

    mutation {
    	verifyLogin(username:"anirudh",password: "asdf")
    }
Copy the token and add an Authorization header in the Headers section to include it on all subsequent requests.

    {
    	"Authorization": <copied token>
    }

###Notes
- Uses model validation attributes to validate inputs to the API.
- If x-correlation-id is present in the request headers, propagates the same to the response. Else creates and includes a new correlation id.