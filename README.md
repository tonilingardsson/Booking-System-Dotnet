# Bokningssystem API

Ett REST-baserat Web API byggt i C# med ASP.NET Core för att hantera kunder, banor och bokningar.

## Beskrivning av systemet och dess arkitektur

Systemet används för att administrera bokningar av sportbanor. En kund kan boka en bana vid en bestämd starttid. Varje bokning är en timme lång.

Systemet består av tre huvudsakliga entiteter:

- **Customer** – innehåller information om kund, exempelvis namn, e-postadress och telefonnummer.
- **Court** – representerar en bokningsbar bana.
- **Booking** – kopplar ihop en kund och en bana vid en viss tidpunkt.

En `Booking` innehåller `CustomerId`, `CourtId` och `StartTime`. Egenskapen `EndTime` beräknas automatiskt till en timme efter starttiden.

### Lager och ansvar

Projektet är delvis uppdelat i lager:

```text
Controllers -> Services -> Entity Framework Core / BookingDbContext -> SQL Server
```

- **Controllers** hanterar HTTP-anrop, routing och HTTP-svar, exempelvis `200 OK`, `201 Created`, `204 No Content`, `400 Bad Request` och `404 Not Found`.
- **Services** innehåller affärslogik för bokningar. `BookingService` kontrollerar bland annat öppettider, dubbelbokningar och att kund och bana finns.
- **Entity Framework Core** används för dataåtkomst och kommunikation med SQL Server via `BookingDbContext`.
- **Models** representerar entiteterna i databasen: `Customer`, `Court` och `Booking`.
- **DTOs** används för create- och update-anrop så att klienten endast skickar de fält som API:t behöver.

### DTO:er

Följande DTO:er används för inkommande create- och update-anrop:

- `CustomerDto`
- `CourtDto`
- `BookingDto`

Exempel på request body för att skapa eller uppdatera en bokning:

```json
{
  "customerId": 1,
  "courtId": 1,
  "startTime": "2026-08-20T15:00:00"
}
```

Klienten skickar inte `Id`, `EndTime`, `Customer` eller `Court`.

### Affärsregler för bokningar

`BookingService` kontrollerar följande regler:

- En bokning måste starta på en hel timme.
- Bokningar är tillåtna mellan 07:00 och 22:00.
- Kunden måste finnas.
- Banan måste finnas.
- En bana kan inte bokas två gånger på samma starttid.
- En bokning är en timme lång.

## API-dokumentation

När projektet körs i development-läge finns Swagger-dokumentationen tillgänglig via:

```text
https://localhost:<port>/swagger
```

Swagger används för att se endpoints, request bodies, response bodies och statuskoder samt för att testa API:t direkt i webbläsaren.

### Customers

| Metod | Endpoint | Beskrivning |
|---|---|---|
| GET | `/api/Customers` | Hämtar alla kunder |
| GET | `/api/Customers/{id}` | Hämtar en kund med angivet ID |
| POST | `/api/Customers` | Skapar en kund |
| PUT | `/api/Customers/{id}` | Uppdaterar en kund |
| DELETE | `/api/Customers/{id}` | Tar bort en kund |

Exempel på `POST /api/Customers`:

```json
{
  "firstName": "Anna",
  "lastName": "Andersson",
  "emailAddress": "anna.andersson@example.com",
  "phoneNumber": "070-123 45 67"
}
```

### Courts

| Metod | Endpoint | Beskrivning |
|---|---|---|
| GET | `/api/Courts` | Hämtar alla banor |
| GET | `/api/Courts/{id}` | Hämtar en bana med angivet ID |
| POST | `/api/Courts` | Skapar en bana |
| PUT | `/api/Courts/{id}` | Uppdaterar en bana |
| DELETE | `/api/Courts/{id}` | Tar bort en bana |

Exempel på `POST /api/Courts`:

```json
{
  "courtName": "Bana 1"
}
```

### Bookings

| Metod | Endpoint | Beskrivning |
|---|---|---|
| GET | `/api/Bookings` | Hämtar alla bokningar |
| GET | `/api/Bookings/{id}` | Hämtar en bokning med angivet ID |
| POST | `/api/Bookings` | Skapar en bokning |
| PUT | `/api/Bookings/{id}` | Uppdaterar en bokning |
| DELETE | `/api/Bookings/{id}` | Tar bort en bokning |
| GET | `/api/Bookings/availability` | Hämtar lediga tider inom ett datumintervall |

Exempel på `POST /api/Bookings`:

```json
{
  "customerId": 1,
  "courtId": 1,
  "startTime": "2026-08-20T15:00:00"
}
```

Exempel på availability-query:

```text
GET /api/Bookings/availability?startDate=2026-08-20&endDate=2026-08-21
```

### Vanliga statuskoder

| Statuskod | Betydelse |
|---|---|
| `200 OK` | Anropet lyckades och returnerar data |
| `201 Created` | En resurs skapades |
| `204 No Content` | Anropet lyckades men returnerar ingen response body |
| `400 Bad Request` | Request body eller affärsregler är ogiltiga |
| `404 Not Found` | Resursen med angivet ID finns inte |

## Beskrivning av implementerade tester

Projektet har testats med automatiserade enhetstester, Swagger och Postman.

### Enhetstester

Automatiserade enhetstester har implementerats med Microsofts testramverk. Testerna kördes med grönt resultat.

Tester används för att verifiera utvald funktionalitet och affärslogik, utan att hela API:t behöver testas manuellt varje gång.

### Swagger

Swagger användes för manuell testning av enskilda endpoints.

Följande kontrollerades:

- Skapa, hämta, uppdatera och ta bort resurser.
- Att `POST` returnerar `201 Created`.
- Att `PUT` returnerar rätt statuskod.
- Att DTO:er visar mindre och tydligare request bodies.
- Att ogiltiga DTO-data returnerar `400 Bad Request`.
- Att validering fungerar, exempelvis att `CourtName` måste innehålla minst två tecken.
- Att en uppdaterad resurs kan hämtas igen med korrekt uppdaterade värden.

### Postman

Postman användes för att testa sammanhängande API-flöden.

Exempel på testflöde:

1. Skapa en kund.
2. Spara kundens ID i en Postman-variabel.
3. Skapa en bana.
4. Spara banans ID i en Postman-variabel.
5. Skapa en bokning med kundens och banans ID.
6. Hämta bokningen.
7. Uppdatera bokningen.
8. Hämta bokningen igen och kontrollera att den uppdaterades.
9. Ta bort bokningen.

Postman användes även för att kontrollera statuskoder, response bodies och hantering av felaktiga anrop.
