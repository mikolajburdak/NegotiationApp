# Price Negotiation Application

## Overview
This is a web application that allows users to manage product negotiations with features for creating products, starting negotiations, making price proposals, and accepting or rejecting offers. It utilizes **ASP.NET Core**, **Entity Framework Core**, **JWT Authentication**, and **SQLite** for the database.

## Features
- **Product Management**: Add, delete, and retrieve products.
- **Negotiation Flow**: Start a negotiation, propose prices, accept/reject offers.
- **Authentication**: User login and registration with JWT-based authentication.

## Technologies Used
- **ASP.NET Core 9**
- **Entity Framework Core** with SQLite
- **JWT Authentication**
- **AutoMapper** for DTO mapping
- **Swagger** for API documentation

## Setup

### Prerequisites
- **.NET 9 SDK**
- **SQLite** (for local database)
- **Visual Studio**, **Rider** or **VS Code** (optional, but recommended)

### Steps to Run the Project Locally

1. **Clone the repository**:
    ```bash
    git clone https://github.com/mikolajburdak/NegotiationApp.git
    cd PriceNegotiationApp
    ```

2. **Restore dependencies**:
    ```bash
    dotnet restore
    ```

3. **Run the application**:
    ```bash
    dotnet run
    ```

4. Open your browser and go to `http://localhost:5238` to access the application.

5. **Database Configuration**:
   The application uses **SQLite** for the database. The database is automatically created when the application starts.

### Swagger API Documentation

Once the application is running, you can access the Swagger UI by navigating to `http://localhost:5238/swagger` to view and test the API endpoints.

## Endpoints

### Auth
- **POST /api/auth/register** - Register a new user
- **POST /api/auth/login** - Log in and receive a JWT token

### Products
- **POST /api/product** - Create a new product (Requires Authentication)
- **GET /api/product** - Get a List of products
- **GET /api/product/{productId}** - Get a product by ID
- **GET /api/product/by-name/{productName}** - Get a product by name
- **DELETE /api/product/{productId}** - Delete a product by ID (Requires Authentication)

### Negotiations
- **POST /api/negotiation** - Start a negotiation (Requires Authentication)
- **POST /api/negotiation/propose/{negotiationId}** - Propose a price (Requires Authentication)
- **POST /api/negotiation/accept/{negotiationId}** - Accept a proposal (Requires Authentication)
- **POST /api/negotiation/reject/{negotiationId}** - Reject a proposal (Requires Authentication)
- **GET /api/negotiation/{negotiationId}** - Get negotiation details
- **DELETE /api/negotiation/{negotiationId}** - Delete a negotiation (Requires Authentication)

## Testing

### Unit and Integration Tests
The application includes unit and integration tests to ensure the correctness of the application.

**To run the tests**
```bash
dotnet test
```