# Hotel Booking API

## 📌 Overview

This is a RESTful API for managing hotel bookings, rooms, and reservations. It provides endpoints to create, update, retrieve, and delete hotels, rooms, and reservations.

## 🏗️ Tech Stack
- .NET 8.0 (ASP.NET Core Web API)
- Entity Framework Core (EF Core)
- SQL Server
- Swagger (NSwag) for API documentation
- C# for backend development

## 🚀 Features

✅ Hotels Management (CRUD operations, activation/deactivation).

✅ Rooms Management (CRUD operations, room activation, pricing).

✅ Reservations (Booking, updating, canceling, conflict prevention).

✅ Validation of inputs (e.g., check-in date must be before check-out date).

✅ Swagger Documentation for API testing .

### 🏨 Hotels Endpoints

| Method    | Endpoint             | Description                        |
| --------- | -------------------- | ---------------------------------- |
| **GET**   | `/hotel`             | Get all hotels (active by default) |
| **GET**   | `/hotel/{id}`        | Get a specific hotel by ID         |
| **POST**  | `/hotel`             | Create a new hotel                 |
| **PATCH** | `/hotel/{id}/status` | Activate/Deactivate a hotel        |

🔹 Example Request (Create Hotel)
{
  "id": 1,
  "name": "Hotel Paradise",
  "address": "123 Main Street",
  "city": "Metropolis",
  "isActive": true
}

🔹 Example Response (Success)
{
  "id": 1,
  "name": "Hotel Paradise",
  "address": "123 Main Street, Cityville",
  "city": "Metropolis",
  "isActive": true,
  "rooms": []
}

### 🏠 Rooms Endpoints

| Method    | Endpoint             | Description                        |
| --------- | -------------------- | ---------------------------------- |
| **GET**   | `/room/{hotelId}`    | Get all rooms for a hotel          |
| **GET**   | `/room/byId/{id}`    | Get a specific room by ID          |
| **POST**  | `/room`              | Create a new room                  |
| **PUT**   | `/room/{id}`         | Update an existing room            |
| **PATCH** | `room/{id}/status`   | Activate/Deactivate a room         |

🔹 Example Request (Create Room)
{
  "hotelId": 1,
  "type": "Suite",
  "basePrice": 250000.00,
  "taxes": 50000.00,
  "location": "Floor 5",
  "isActive": true
}

🔹 Example Response (Success)
{
  "id": 1,
  "hotelId": 1,
  "type": "Suite",
  "basePrice": 250000.00,
  "taxes": 50000.00,
  "location": "Floor 5",
  "isActive": true
}

### 🏷️ Reservations Endpoints

| Method    | Endpoint             | Description                        |
| --------- | -------------------- | ---------------------------------- |
| **GET**   | `/reservation/{roomId`| Get all reservations for a room        |
| **GET**   | `/reservation/id/{id}`| Get a specific reservation by ID         |
| **POST**  | `/reservation`        | Create a new reservation                 |
| **PUT**   | `/reservation/{id}`   | Update a reservation          |
| **DELETE**| `/reservation/{id}`   | Cancel a reservation        |

🔹 Example Request (Create Reservation)

{
  "roomId": 1,
  "guestFullName": "John Doe",
  "guestEmail": "johndoe@email.com",
  "checkIn": "2024-07-10T14:00:00",
  "checkOut": "2024-07-15T12:00:00",
  "totalPrice": 300000.00
}

🔹 Example Response (Success)

{
  "id": 1,
  "roomId": 1,
  "guestFullName": "John Doe",
  "guestEmail": "johndoe@email.com",
  "checkIn": "2024-07-10T14:00:00",
  "checkOut": "2024-07-15T12:00:00",
  "totalPrice": 300000.00
}

## 🔧 Setup & Installation

### 🛠 Prerequisites

.NET 8.0 SDK installed ([Download](https://dotnet.microsoft.com/en-us/download/dotnet/8.0))

SQL Server

## 🚀 Steps to Run

### 1️⃣ Clone this repository:

git clone https://github.com/your-username/hotel-booking-api.git
cd hotel-booking-api

### 2️⃣ Install dependencies:

dotnet restore

### 3️⃣ Configure the database connection in appsettings.json

"ConnectionStrings": {
  "DefaultConnection": "Server=your-server;Database=HotelBooking;User Id=your-user;Password=your-password;"
}

### 4️⃣ Run migrations & update database:

dotnet ef database update

### 5️⃣ Start the API:

dotnet run

### 6️⃣ Open Swagger UI for API testing:
📌 http://localhost:5000/swagger