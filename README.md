# Products Management API

A RESTful API built with ASP.NET Core using Clean Architecture, SOLID principles, and modern development practices.
This project demonstrates the usage of Entity Framework Core, AutoMapper, Dependency Injection (DI), Factory Pattern, Service/Repository Pattern, Refit, MSTest2, AutoFixture, Mocking, and Code-First Migrations.

# Features

✅ RESTful API for managing products
✅ Entity Framework Core with Code-First Migrations
✅ Service & Repository patterns for separation of concerns
✅ Dependency Injection for services, repositories, and factories
✅ AutoMapper for mapping between DTOs and entities
✅ Factory Pattern for creating product-related objects dynamically
✅ Refit for making strongly-typed API calls
✅ MSTest2 framework for unit testing
✅ AutoFixture & Moq for mocking dependencies in tests
✅ LINQ for efficient data querying
✅ Swagger/OpenAPI for API documentation

# Technologies Used

| Technology / Library           | Purpose                                             |
| ------------------------------ | --------------------------------------------------- |
| **ASP.NET Core 8+**            | Building RESTful APIs                               |
| **Entity Framework Core**      | ORM for database access                             |
| **AutoMapper**                 | Object-to-object mapping between DTOs and Entities  |
| **Dependency Injection**       | Built-in DI container for managing dependencies     |
| **Factory Pattern**            | Creating objects dynamically based on conditions    |
| **Service/Repository Pattern** | Encapsulation of business logic and data access     |
| **Refit**                      | Simplified HTTP client for API-to-API communication |
| **MSTest2**                    | Unit testing framework                              |
| **AutoFixture**                | Test data generation                                |
| **Moq**                        | Mocking dependencies in unit tests                  |
| **Swagger**                    | API documentation                                   |
| **LINQ**                       | Simplified data filtering, sorting, and selection   |

# Project Structure

ProductsManagementAPI
├── PM.API                       # API layer (Controllers)
├── PM.Application               # Application layer (Services , Core business)
├── PM.Common                    # Common layer (DTOs, Repositories, Interfaces)
├── PM.EnityFrameworkCore        # Entities ,DB Context, Migrations
├── PM.Tests                     # Unit tests (MSTest2, AutoFixture, Moq)
└── README.md

# API Endpoints

| Method     | Endpoint             | Description             | Request Body |
| ---------- | -------------------- | ----------------------- | ------------ |
| **GET**    | `/api/products`      | Get all products        | —            |
| **GET**    | `/api/products/{id}` | Get product by ID       | —            |
| **POST**   | `/api/products`      | Create a new product    | JSON         |
| **PUT**    | `/api/products/{id}` | Update existing product | JSON         |
| **DELETE** | `/api/products/{id}` | Delete product          | —            |
| **GET**    | `/api/ExchangeRate/latest?baseCurrency=USD`      | Get ExchangeRates 

# Unit Testing Tools

MSTest2 → Primary testing framework
AutoFixture → Auto-generates sample data
Moq → Mocks dependencies like repositories and services
