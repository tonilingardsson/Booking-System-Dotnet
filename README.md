# Booking System API

## Project purpose

This project is an ASP.NET Core Web API for managing court bookings. It allows clients to create and read bookings while validating key business rules before data is saved.

## Tech stack

* **.NET** (ASP.NET Core Web API)
* **Entity Framework Core**
* **SQL Server**
* **Swagger / OpenAPI**
* **MSTest**
* **Postman**

## Prerequisites

Before running the project, make sure the following are installed:

- .NET SDK
- SQL Server
- Entity Framework Core tools (optional, for running migrations)

## POST responses

When a resource is successfully created, the API returns `201 Created`. In ASP.NET Core, this can be implemented with `CreatedAtAction(...)` so the response includes both the created object and a reference to where it can be retrieved.

## Court data

The project includes a `Court` entity. At the moment, courts are stored as seed data and used when creating bookings.

___

## How to Run the Project

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/tonilingardsson/Booking-System-Dotnet.git](https://github.com/tonilingardsson/Booking-System-Dotnet.git)
   ```
2. **Navigate to the API project folder:**
   ```bash
   cd Booking-System-Dotnet/Booking_System.Api
   ```
3. Restore dependencies:
```bash
   dotnet restore
   ```
4. Apply database migrations:
Ensure your SQL Server instance is running and your connection string in appsettings.json is correctly configured. Then, update the database:
```bash
   dotnet ef database update
   ```
5. Run the application:
```bash
   dotnet run
   ```

## Base URL and Swagger URL 

Once the application is running, the API will be hosted locally.

* **Base API URL:** https://localhost:5001/api (Port may vary depending on your launchSettings.json configuration, e.g., 7001 or 7123).
* **Swagger Documentation:** https://localhost:5001/swagger

Swagger can be used directly in your browser to explore the endpoints, view schemas, and execute test requests.

## Available endpoints:
**Bookings:**
    * GET /api/Bookings
    * GET /api/Bookings/{id}
    * POST /api/Bookings
    * PUT /api/Bookings/{id}
    * DELETE /api/Bookings/{id}

***Note:startTime should be sent in ISO 8601 format, for example "2026-07-20T14:00:00".

**Customers:**
    * GET /api/Customers
    * GET /api/Customers/{id}
    * POST /api/Customers
    * PUT /api/Customers/{id}
    * DELETE /api/Customers/{id}


## Endpoints & JSON Examples
### Customers
Create Customer: POST /api/Customers

Request Body:

JSON
```json
{
  "name": "Jane Doe",
  "email": "jane.doe@example.com",
  "phoneNumber": "070-1234567"
}
```

Response (201 Created):

JSON
```json
{
  "id": 1,
  "name": "Jane Doe",
  "email": "jane.doe@example.com",
  "phoneNumber": "070-1234567"
}
```

### Bookings
**Create Booking:** POST /api/Bookings

Note: Bookings must start on a whole hour (e.g., 14:00) and fall within the 07:00 - 21:00 operating window.

Request Body:

JSON
```json
{
  "customerId": 1,
  "courtId": 1,
  "startTime": "2026-07-20T14:00:00"
}
```

Response (201 Created):

JSON
```json
{
  "id": 1,
  "customerId": 1,
  "courtId": 1,
  "startTime": "2026-07-20T14:00:00",
  "endTime": "2026-07-20T15:00:00"
}
```
Validation Error Response (400 Bad Request) - Example of booking at an invalid time:

JSON
```json
{
  "status": 400,
  "title": "Validation Error",
  "detail": "A booking must start on a whole hour."
}
```
## Validation Rules Implemented

The current booking validation is handled in the service layer using the ValidateBookingAsync method.

* Bookings must start on a whole hour (e.g., 13:00).
* Valid start times are between 07:00 and 21:00 (making the end time no later than 22:00).
* The selected customer and court must exist in the database.
* A court cannot be double-booked for the exact same start time.

Because EndTime is derived from StartTime + 1 hour, the system does not currently need a separate user-input validation to check whether the end time is earlier than the start time.

## Error handling

The current validation flow returns the first relevant error that is found. For example, if the booking time is invalid, the API can reject the request before checking whether the customer exists. This is acceptable for a beginner-friendly backend API because the validation order is consistent and easy to explain.

## Current limitations

The current overlap validation checks whether another booking exists on the same court with the exact same start time. This means the implementation currently prevents double-booking for identical one-hour slots, but broader overlap logic is only fully correct because each booking is fixed to one hour.

There is also a TODO left in the code for availability calculation. That feature has not yet been completed and should be documented as planned work rather than finished functionality.

## API testing
The API is tested at two levels:

### Unit tests
Unit tests should focus on the validation logic in the booking service. The assignment requires at least 10 unit tests, and MSTest is a suitable choice for a Visual Studio based .NET project.[3][4]

Suggested unit test coverage:

1. Valid booking at `07:00` should pass.
2. Valid booking at `21:00` should pass.
3. Booking at `06:00` should fail.
4. Booking at `22:00` should fail.
5. Booking at `13:30` should fail because it is not on a whole hour.
6. Booking with a non-existing customer should fail.
7. Booking with a non-existing court should fail.
8. Booking with the same court and same start time as an existing booking should fail.
9. Booking with the same time but a different court should pass.
10. Booking with valid customer, valid court, and valid time should pass.

## Integration tests
Postman should be used for integration testing of critical endpoint flows.

Suggested Postman flows:
- `GET /api/Bookings` returns `200 OK`.
- `POST /api/Bookings` with valid data returns `201 Created`.
- `POST /api/Bookings` with a non-existing customer returns a validation error.
- `POST /api/Bookings` with a non-existing court returns a validation error.
- `POST /api/Bookings` with a non-whole-hour time returns a validation error.
- `POST /api/Bookings` with an already booked court and time returns a validation error.
