# **Hotel Booking API** 🏨

## 📌 **Description**
Hotel Booking API is a **RESTful API** for managing hotels, rooms, and reservations. It provides endpoints to **create, update, retrieve, and delete** hotels, rooms, and reservations efficiently.

🌐 **Swagger Documentation**: [Swagger UI](https://hotelbookingazure.azurewebsites.net/swagger/index.html)

---

## 🏠 **Technologies Used**
- ⚡ **.NET 8.0 (ASP.NET Core Web API)**
- 💪 **C# for Backend**
- 💪 **Entity Framework Core (EF Core)**
- 📂 **SQL Server**
- 🔎 **Swagger (NSwag) for Documentation**
- ✨ **JWT for Authentication and Authorization**

---

## ⚖️ **Authentication & Authorization**
This API uses **JWT (JSON Web Tokens)** to manage authentication and access control.

### 🔑 **Available Roles**
- ✨ **Admin**: Can manage hotels and rooms.
- 🏡 **Guest**: Can search hotels, make reservations, and cancel them.

🔗 **To obtain a JWT token:**
1. **Authenticate at `/api/auth/login`** with a registered user.
2. Use the `token` for requests to protected endpoints.

```json
{
    "username": "adminuser",
    "password": "password123"
}
```

**Example JWT Token Usage:**
```http
Authorization: Bearer eyJhbGciOiJIUzI1...
```

---

## 🚀 **Features**
✔️ **Hotel Management** (CRUD, activation & deactivation).  
✔️ **Room Management** (CRUD, availability validation).  
✔️ **Reservations** (Create, update, cancel, avoid duplicates).  
✔️ **Advanced validations** (dates, room capacity, occupancy).  
✔️ **JWT Authentication** for security.  
✔️ **Interactive Documentation with Swagger**.  

---

## 📂 **API Endpoints Documentation**

### 🏨 **Hotel Management**
| Method  | Endpoint             | Description |
|---------|---------------------|-------------|
| **GET** | `/api/hotel`        | Retrieve all active hotels. |
| **GET** | `/api/hotel/{id}`   | Retrieve a hotel by ID. |
| **POST** | `/api/hotel`        | Create a new hotel (**Admin Only**). |
| **PATCH** | `/api/hotel/{id}/status` | Activate/Deactivate a hotel. |

**Example (Create Hotel)**
```json
{
  "name": "Hotel Paradise",
  "address": "Street 123",
  "city": "Bogota",
  "isActive": true
}
```

---

### 🏡 **Room Management**
| Method  | Endpoint          | Description |
|---------|----------------|-------------|
| **GET** | `/api/room/{hotelId}`  | Retrieve rooms of a hotel. |
| **POST** | `/api/room`  | Create a room (**Admin Only**). |
| **PUT** | `/api/room/{id}` | Update a room. |
| **PATCH** | `/api/room/{id}/status` | Activate/Deactivate a room. |

**Example (Create a Room)**
```json
{
  "hotelId": 3,
  "type": "Suite",
  "basePrice": 350000.00,
  "taxes": 70000.00,
  "location": "Floor 8",
  "capacity": 4,
  "isActive": true
}
```

---

### 🗃️ **Reservation Management**
| Method  | Endpoint                | Description |
|---------|------------------------|-------------|
| **POST** | `/api/reservation`     | Create a reservation (**Guest Only**). |
| **PUT** | `/api/reservation/{id}` | Update a reservation. |
| **DELETE** | `/api/reservation/{id}` | Cancel a reservation. |

**Example (Create Reservation)**
```json
{
  "roomId": 3,
  "checkInDate": "2025-07-20",
  "checkOutDate": "2025-07-25",
  "emergencyContactName": "Carlos Martinez",
  "emergencyContactPhone": "+573002345678",
  "guests": [
    {
      "firstName": "Andres",
      "lastName": "Solano",
      "email": "andresolano@gmail.com",
      "dateOfBirth": "1999-10-06",
      "documentNumber": "1010125555",
      "documentType": 2,
      "gender": 2,
      "phoneNumber": "+573213941848"
    }
  ]
}
```

---

## 🔧 **Installation & Setup**
### ⚡ **Requirements**
1. **Install .NET 8.0 SDK** → [Download](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
2. **Configure the database in `appsettings.json`**
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=your-server;Database=HotelBooking;User Id=your-user;Password=your-password;"
}
```

### ⏳ **Steps to Run**
```sh
git clone https://github.com/AndresSolano06/HotelBookingApp.git
cd HotelBookingApp
dotnet restore
dotnet ef database update
dotnet run
```
