# **Hotel Booking API** 🏨

## 📌 **Descripción**
Hotel Booking API es una **API RESTful** para la gestión de hoteles, habitaciones y reservas. Proporciona endpoints para **crear, actualizar, obtener y eliminar** hoteles, habitaciones y reservas de manera eficiente.

🌐 **Documentación Swagger**: [Swagger UI](https://hotelbookingazure.azurewebsites.net/swagger/index.html)

---

## 🏰 **Tecnologías Utilizadas**
- ⚡ **.NET 8.0 (ASP.NET Core Web API)**
- 💪 **C# para el Backend**
- 💪 **Entity Framework Core (EF Core)**
- 📂 **SQL Server**
- 🔎 **Swagger (NSwag) para Documentación**
- ✨ **JWT para Autenticación y Autorización**

---

## ⚖️ **Autenticación y Autorización**
Esta API utiliza **JWT (JSON Web Tokens)** para gestionar la autenticación y el acceso a los endpoints.

### 🔑 **Roles Disponibles**
- ✨ **Admin**: Puede gestionar hoteles y habitaciones.
- 🛌 **Guest**: Puede buscar hoteles, hacer reservas y cancelarlas.

🔗 **Para obtener un token JWT:**
1. **Autenticarse en `/api/auth/login`** con un usuario registrado.
2. Usar el `token` en las peticiones a endpoints protegidos.

```json
{
    "username": "adminuser",
    "password": "password123"
}
```

**Ejemplo de Token JWT:**
```http
Authorization: Bearer eyJhbGciOiJIUzI1...
```

---

## 🚀 **Características**
✔️ **Gestión de Hoteles** (CRUD, activación y desactivación).  
✔️ **Gestión de Habitaciones** (CRUD, validación de disponibilidad).  
✔️ **Reservas** (Crear, actualizar, cancelar, evitar duplicados).  
✔️ **Validaciones avanzadas** (fechas, capacidad de habitaciones, ocupación).  
✔️ **Autenticación con JWT** para seguridad.  
✔️ **Documentación interactiva con Swagger**.  

---

## 📂 **Documentación de Endpoints**

### 🏨 **Gestión de Hoteles**
| Método  | Endpoint             | Descripción |
|---------|---------------------|-------------|
| **GET** | `/api/hotel`        | Obtener todos los hoteles activos. |
| **GET** | `/api/hotel/{id}`   | Obtener un hotel por ID. |
| **POST** | `/api/hotel`        | Crear un nuevo hotel (**Admin Only**). |
| **PATCH** | `/api/hotel/{id}/status` | Activar/Desactivar un hotel. |

**Ejemplo (Crear Hotel)**
```json
{
  "name": "Hotel Paradise",
  "address": "Calle 123",
  "city": "Bogotá",
  "isActive": true
}
```

---

### 🏠 **Gestión de Habitaciones**
| Método  | Endpoint          | Descripción |
|---------|----------------|-------------|
| **GET** | `/api/room/{hotelId}`  | Obtener habitaciones de un hotel. |
| **POST** | `/api/room`  | Crear una habitación (**Admin Only**). |
| **PUT** | `/api/room/{id}` | Actualizar una habitación. |
| **PATCH** | `/api/room/{id}/status` | Activar/Desactivar una habitación. |

**Ejemplo (Crear una Habitación)**
```json
{
  "hotelId": 3,
  "type": "Suite",
  "basePrice": 350000.00,
  "taxes": 70000.00,
  "location": "Piso 8",
  "capacity": 4,
  "isActive": true
}
```

---

### 🗃️ **Gestión de Reservas**
| Método  | Endpoint                | Descripción |
|---------|------------------------|-------------|
| **POST** | `/api/reservation`     | Crear una reserva (**Guest Only**). |
| **PUT** | `/api/reservation/{id}` | Actualizar una reserva. |
| **DELETE** | `/api/reservation/{id}` | Cancelar una reserva. |

**Ejemplo (Crear Reserva)**
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

## 🔧 **Instalación y Configuración**
### ⚡ **Requisitos**
1. **Instalar .NET 8.0 SDK** → [Descargar](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
2. **Configurar la base de datos en `appsettings.json`**
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=your-server;Database=HotelBooking;User Id=your-user;Password=your-password;"
}
```

### ⏳ **Pasos para ejecutar**
```sh
git clone https://github.com/AndresSolano06/HotelBookingApp.git
cd HotelBookingApp
dotnet restore
dotnet ef database update
dotnet run
```

---

## 📚 **Licencia**
Este proyecto está licenciado bajo **MIT License** - Ver el archivo [LICENSE](LICENSE).

