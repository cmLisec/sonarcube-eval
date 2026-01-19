# ProductApi

A simple ASP.NET Core Web API for managing products with full CRUD operations.

## Features

- ? RESTful API for Product management
- ? In-memory data store with static initial data
- ? Full CRUD operations (Create, Read, Update, Delete)
- ? Swagger/OpenAPI documentation
- ? Comprehensive unit tests using NUnit and Moq
- ? Integration tests with WebApplicationFactory
- ? GitHub Actions CI/CD pipeline
- ? Code coverage reporting

## Project Structure

```
ng-sonarqube-eval/
??? .github/
?   ??? workflows/
?       ??? build-and-test.yml       # GitHub Actions workflow
??? src/
?   ??? ProductApi.API/              # Main API project
?       ??? Controllers/
?       ?   ??? ProductsController.cs
?       ??? Models/
?       ?   ??? Product.cs
?       ??? Services/
?       ?   ??? IProductService.cs
?       ?   ??? ProductService.cs
?       ??? Program.cs
??? tests/
    ??? ProductApi.Tests/            # Test project
        ??? Controllers/
        ?   ??? ProductsControllerTests.cs
        ??? Services/
        ?   ??? ProductServiceTests.cs
        ??? Integration/
            ??? ProductsIntegrationTests.cs
```

## Getting Started

### Prerequisites

- .NET 10.0 SDK or later

### Running the API

```bash
cd src/ProductApi.API
dotnet run
```

The API will be available at:
- HTTPS: https://localhost:5001
- HTTP: http://localhost:5000
- Swagger UI: https://localhost:5001/swagger

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with code coverage
dotnet test --collect:"XPlat Code Coverage"
```

## API Endpoints

### Get All Products
```
GET /api/products
```

### Get Product by ID
```
GET /api/products/{id}
```

### Create Product
```
POST /api/products
Content-Type: application/json

{
  "name": "Product Name",
  "description": "Product Description",
  "price": 99.99,
  "stockQuantity": 10
}
```

### Update Product
```
PUT /api/products/{id}
Content-Type: application/json

{
  "name": "Updated Name",
  "description": "Updated Description",
  "price": 149.99,
  "stockQuantity": 20
}
```

### Delete Product
```
DELETE /api/products/{id}
```

## Test Coverage

The project includes comprehensive test coverage:

- **Unit Tests**: Testing individual components (Services, Controllers)
- **Integration Tests**: End-to-end API testing
- **Mocking**: Using Moq for dependency isolation

### Test Statistics
- Service Tests: 16 tests covering all CRUD operations
- Controller Tests: 15 tests covering all endpoints and edge cases
- Integration Tests: 10 tests covering full API workflows

## CI/CD Pipeline

The GitHub Actions workflow automatically:
- ? Builds the solution on every PR and push to main
- ? Runs all tests
- ? Generates code coverage reports
- ? Uploads test results and coverage reports as artifacts

## Technologies Used

- **ASP.NET Core 10.0** - Web API framework
- **Swashbuckle.AspNetCore** - Swagger/OpenAPI documentation
- **NUnit** - Testing framework
- **Moq** - Mocking framework
- **Microsoft.AspNetCore.Mvc.Testing** - Integration testing
- **GitHub Actions** - CI/CD pipeline

## License

This project is created for evaluation purposes.