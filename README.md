# **Hotel Booking API** 🏨

## 📌 **Overview**
Hotel Booking API is a **RESTful API** designed to manage hotels, rooms, and reservations. It provides endpoints to **create, update, retrieve, and delete** hotels, rooms, and reservations efficiently.

🌐 **Live API Documentation**: [Swagger UI](https://hotelbookingazure.azurewebsites.net/swagger/index.html)

---

## 🏗️ **Tech Stack**
- ⚡ **.NET 8.0 (ASP.NET Core Web API)**
- 🛢 **Entity Framework Core (EF Core)**
- 💾 **SQL Server**
- 🔍 **Swagger (NSwag) for API Documentation**
- 🚀 **C# for Backend Development**

---

## 🚀 **Features**
✔️ **Hotels Management** (CRUD operations, activation/deactivation).  
✔️ **Rooms Management** (CRUD operations, room pricing & availability).  
✔️ **Reservations** (Booking, updating, cancellation, conflict prevention).  
✔️ **Input Validation** (e.g., check-in date must be before check-out date).  
✔️ **Swagger Documentation** for easy API testing.  

---

## 📌 **Endpoints Documentation**
### 🏨 **Hotels**
| Method  | Endpoint                 | Description                          |
|---------|--------------------------|--------------------------------------|
| **GET** | `/api/hotel`             | Retrieve all hotels (active by default). |
| **GET** | `/api/hotel/{id}`        | Retrieve a specific hotel by ID. |
| **POST** | `/api/hotel`            | Create a new hotel. |
| **PATCH** | `/api/hotel/{id}/status` | Activate/Deactivate a hotel. |

🔹 **Example Request (Create a Hotel)**
```json
{
  "name": "Hotel Paradise",
  "address": "125 Main Street",
  "city": "Cali",
  "isActive": true
}
```
🔹 **Example Response**
```json
{
  "id": 3,
  "name": "Hotel Paradise",
  "address": "125 Main Street",
  "city": "Cali",
  "isActive": true,
  "rooms": []
}
```

---

### 🏠 **Rooms**
| Method  | Endpoint                 | Description                          |
|---------|--------------------------|--------------------------------------|
| **GET** | `/api/room/{hotelId}`    | Retrieve all rooms for a hotel. |
| **GET** | `/api/room/byId/{id}`    | Retrieve a specific room by ID. |
| **POST** | `/api/room`             | Create a new room. |
| **PUT** | `/api/room/{id}`         | Update an existing room. |
| **PATCH** | `/api/room/{id}/status` | Activate/Deactivate a room. |

🔹 **Example Request (Create a Room)**
```json
{
  "hotelId": 3,
  "type": "Suite",
  "basePrice": 300000.00,
  "taxes": 60000.00,
  "location": "Floor 7",
  "capacity": 5,
  "isActive": true
}

```
🔹 **Example Response**
```json
{
  "id": 3,
  "hotelId": 3,
  "type": "Suite",
  "basePrice": 300000.00,
  "taxes": 60000.00,
  "location": "Floor 7",
  "isActive": true,
  "capacity": 5
}
```

---

### 🏷️ **Reservations**
| Method  | Endpoint                    | Description                        |
|---------|-----------------------------|------------------------------------|
| **GET** | `/api/reservation/{roomId}` | Retrieve all reservations for a room. |
| **GET** | `/api/reservation/id/{id}`  | Retrieve a specific reservation by ID. |
| **POST** | `/api/reservation`         | Create a new reservation. |
| **PUT** | `/api/reservation/{id}`     | Update an existing reservation. |
| **DELETE** | `/api/reservation/{id}`  | Cancel a reservation. |

🔹 **Example Request (Create a Reservation)**
```json
{
  "roomId": 3,
  "checkIn": "2024-07-20T14:00:00",
  "checkOut": "2024-07-25T12:00:00",
  "totalPrice": 500000.00,
  "emergencyContactName": "Maria Gonzalez",
  "emergencyContactPhone": "+573001234567",
  "guests": [
    {
      "fullName": "Andres Solano",
      "email": "andresolano.12.651@gmail.com",
      "dateOfBirth": "1999-10-06",
      "documentNumber": "1010125555",
      "documentType": "CC",
      "gender": "Male",
      "phoneNumber": "+573213941848"
    },
    {
      "fullName": "Juan Perez",
      "email": "juan.perez@email.com",
      "dateOfBirth": "1995-08-20",
      "documentNumber": "987654321",
      "documentType": "CC",
      "gender": "Male",
      "phoneNumber": "+573001112233"
    }
  ]
}

```
🔹 **Example Response**
```json
{
    "id": 2,
    "roomId": 3,
    "checkIn": "2024-07-20T14:00:00",
    "checkOut": "2024-07-25T12:00:00",
    "totalPrice": 500000,
    "emergencyContactName": "Maria Gonzalez",
    "emergencyContactPhone": "+573001234567",
    "guests": [
        {
            "id": 3,
            "fullName": "Andres Solano",
            "email": "andresolano.12.651@gmail.com",
            "dateOfBirth": "1999-10-06T00:00:00",
            "gender": "Male",
            "documentType": "CC",
            "documentNumber": "1010125555",
            "phoneNumber": "+573213941848",
            "reservationId": 2
        },
        {
            "id": 4,
            "fullName": "Juan Perez",
            "email": "juan.perez@email.com",
            "dateOfBirth": "1995-08-20T00:00:00",
            "gender": "Male",
            "documentType": "CC",
            "documentNumber": "987654321",
            "phoneNumber": "+573001112233",
            "reservationId": 2
        }
    ]
}
```

---

## 🔧 **Setup & Installation**
### 🛠 **Prerequisites**
1️⃣ **Install .NET 8.0 SDK** → [Download](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
2️⃣ **Install SQL Server**  
3️⃣ **Ensure EF Core is installed** (if using migrations)

---

## 🚀 **Steps to Run the API**
### 1️⃣ Clone this repository:
```sh
git clone https://github.com/AndresSolano06/HotelBookingApp.git
cd HotelBookingApp
```
### 2️⃣ Install dependencies:
```sh
dotnet restore
```
### 3️⃣ Configure the database connection in `appsettings.json`
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=your-server;Database=HotelBooking;User Id=your-user;Password=your-password;"
}
```
### 4️⃣ Run migrations & update database:
```sh
dotnet ef database update
```
### 5️⃣ Start the API:
```sh
dotnet run
```
### 6️⃣ Open Swagger UI for API testing:
📌 [https://localhost:7165/swagger](https://localhost:7165/swagger)  

---

## 🔥 **Deployment**
### **To Azure App Service**
1. Open Visual Studio.
2. Right-click on the project → **Publish**.
3. Select **Azure App Service**.
4. Deploy & access Swagger:  
   👉 **[https://hotelbookingazure.azurewebsites.net/swagger/index.html](https://hotelbookingazure.azurewebsites.net/swagger/index.html)**

---

## 📜 **License**
This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

