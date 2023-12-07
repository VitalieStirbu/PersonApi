# PersonAPI

## Description
PersonAPI is a RESTful WebAPI built on .NET Core 7 that allows interaction with a in memory database of person records. 

## Features
- CRUD operations for managing person records
- Logging using Serilog for detailed information and error handling
- Request validation implemented with FluentValidation for data integrity
- Swagger integration for API documentation and testing

## Setup Instructions
1. Clone the repository.
2. Ensure you have .NET Core 7 SDK installed.
3. Navigate to the project directory in the terminal.
4. Run `dotnet build` to build the solution.
5. Run `dotnet run` to start the API.
 OR
4. Run the project from VS

## Logging
- Serilog is used for logging.
- Logs are stored in the `logs` folder.
- Logs can also be viewed in the console.
- Logging configuration can be adjusted in the `appsettings.json` file.

## Validation
- FluentValidation is implemented for request validation.
- Validators for different endpoints can be found in the Validators folder.

## Swagger
- Swagger is integrated to provide API documentation and a user-friendly interface for testing endpoints.
- Access the Swagger UI by navigating to `/swagger` endpoint in your browser after starting the API.

## Endpoints
- `GET /api/persons`: Retrieve all person records.
- `GET /api/persons/{id}`: Retrieve a specific person record by ID.
- `POST /api/persons`: Create a new person record.
- `PUT /api/persons/{id}`: Update an existing person record.
- `DELETE /api/persons/{id}`: Delete a person record by ID.

## Dependencies
- .NET Core 7
- Serilog
- FluentValidation
- Swagger