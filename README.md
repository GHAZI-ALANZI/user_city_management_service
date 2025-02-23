# 🚀 User City Management API


## 📌 Overview
User Management API is a **secure RESTful API** built with **ASP.NET Core 8** that provides user authentication, registration, deletion, and banning functionalities. It follows **Domain-Driven Design (DDD)** and includes **JWT-based authentication**.



## 🛠️ **Technologies Used**
- **ASP.NET Core 8**
- **Entity Framework Core (EF Core)**
- **Domain-Driven Design (DDD)**
- **JWT Authentication**
- **AutoMapper**
- **Moq & xUnit (Unit Testing)**
- **Swagger UI (OpenAPI)**
- **Docker Support**



## 📂 **Project Structure**
 UserCityManagementAPI
 
  ── UserManagementAPI/ 
    
     ── Application/ # Business logic (DTOs , Interfaces , Services)
     ── Domain/ # Entities, enums
     ── Infrastructure/ # Data and Repositories implementations
     ── WebAPI/ # Controllers, middleware, authentication
     ── README.md # Documentation 
     ── docker-compose.yml # Docker setup for API and database 
     ── Docker  
     ── ApiDocument   # swagger.yaml # OpenAPI documentationyaml
 
  ── UserManagementAPI.Tests  # Unit and integration tests
  
     ── Controllers  
     



## 📌 **Installation & Setup**

###  **Clone the Repository**

git clone https://github.com/GHAZI-ALANZI/user_city_management_service.git
cd UserManagementAPI

# Set Up Database (SQL Server)

Modify appsettings.json to configure the database connection:

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=UserManagementDB;User Id=sa;Password=YourStrong!Pass;"
}


# Run Database Migrations

dotnet ef migrations add InitialCreate
dotnet ef database update

# Run the API

dotnet run

# 🌍 The API will be available at:
http://localhost:5000

# 📌 Authentication & JWT
This API uses JWT-based authentication.

# 🔑 Login to Get Token

POST /api/auth/login
Content-Type: application/json

{
    "username": "FullAdmin",
    "password": "Admin@123"
}

# Response (Success):


{
    "token": "eyJhbGciOiJIUz..."
}

# 📌 Use the Token in Requests
After login, include the token in the Authorization header:


# Authorization: Bearer YOUR_TOKEN_HERE

#📌 API Endpoints 

#🔑 Authentication**

 Method	    Endpoint	                       Description
 POST	    /api/auth/login	                   Logs in a user and returns a JWT token

# 👤 User Management **

Method	    Endpoint	                       Description
POST	    /api/users/register	               Registers a new user (FullAdmin only)
DELETE	    /api/users/delete/{username}	   Deletes a user (Full Admin only)
POST	    /api/users/ban/{username}	       Bans a user (Full Admin only)
POST        /api/users/unban/{username}        Unbans a user (Full Admin only)
GET         /api/users/all                     Get all Users

# 📌 Unit Testing
This project includes unit tests using xUnit & Moq.

# 🔹 Running Tests

dotnet test



# 📌 Swagger API Documentation
Once the API is running, access Swagger UI at:
🌍 http://localhost:5000/swagger/index.html

For the raw OpenAPI JSON file:
📄 http://localhost:5000/swagger/v1/swagger.json

For the YAML version:
📄 http://localhost:5000/swagger/v1/swagger.yaml


# 📌 Run with Docker
1️⃣ Build and run the container:

docker-compose up --build

# The API will be available at:
🌍 http://localhost:5000

# 📌 Default Admin User
For testing, a Full Admin user is created automatically when the database is initialized:

Username	          Email	              Password	            Role
FullAdmin	          admin@example.com	  Admin@123	            FullAdmin


# 📌 How to Modify Default Admin User

📂 Modify this file: ApplicationDbContext.cs


protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    var fullAdminUser = new User
    {
        Id = Guid.NewGuid(),
        Username = "FullAdmin",
        Email = "admin@example.com",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
        Role = Role.FullAdmin,
        IsBanned = false
    };

    modelBuilder.Entity<User>().HasData(fullAdminUser);
}

# 📌 After modifying, run:


dotnet ef database update


