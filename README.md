# Booking System API

## Project purpose

This project is an ASP.NET Core Web API for managing court bookings. It allows clients to create and read bookings while validating key business rules before data is saved.

## Tech stack

- .NET
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Swagger / OpenAPI
- MSTest
- Postman

Swagger is used to explore and test endpoints in the browser, while Postman is used to verify critical API flows outside the Swagger UI.[1][2]

## Main entities

The API is built around three main entities:

- **Booking** – stores the reservation date and time together with the selected customer and court.
- **Customer** – stores customer information such as name, email address, and phone number.
- **Court** – stores the court that can be booked.

## Validation rules implemented

The current booking validation is handled in the service layer. Based on the current `ValidateBookingAsync` method, these rules are implemented:

- A booking must start on a whole hour, for example `13:00`.
- A booking must be within opening hours.
- Valid start times are from `07:00` to `21:00`, which makes the one-hour booking end no later than `22:00`.
- The selected customer must exist in the database.
- The selected court must exist in the database.
- A court cannot be booked twice for the same start time.

Because `EndTime` is derived from `StartTime + 1 hour`, the system does not currently need a separate user-input validation to check whether end time is earlier than start time.

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

### Integration tests

Postman should be used for integration testing of critical endpoint flows.

Suggested Postman flows:

- `GET /api/Bookings` returns `200 OK`.
- `POST /api/Bookings` with valid data returns `201 Created`.
- `POST /api/Bookings` with a non-existing customer returns a validation error.
- `POST /api/Bookings` with a non-existing court returns a validation error.
- `POST /api/Bookings` with a non-whole-hour time returns a validation error.
- `POST /api/Bookings` with an already booked court and time returns a validation error.

## Error handling

The current validation flow returns the first relevant error that is found. For example, if the booking time is invalid, the API can reject the request before checking whether the customer exists. This is acceptable for a beginner-friendly backend API because the validation order is consistent and easy to explain.
