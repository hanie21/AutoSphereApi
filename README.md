
# AutoSphere API

## Description
AutoSphere API is a .NET Core RESTful Web API designed to handle vehicle-related data with PostgreSQL as the database. This API allows operations like adding, searching, and saving vehicle data. It also integrates with a search engine (e.g., OpenSearch) for advanced searching capabilities, including fuzzy matching for better search results.

The API supports both basic and advanced searching:
- **Basic Searching**: Allows filtering vehicles by parameters like make, model, price, mileage, etc.
- **Advanced Searching**: Includes advanced filtering and fuzzy matching for more accurate results when dealing with misspelled or similar search terms.

## Technologies Used

- **Backend Framework**: .NET Core (ASP.NET Core)
- **Database**: PostgreSQL
- **Search Engine**: OpenSearch (for indexing and searching vehicles)
- **Caching**: MemoryCache (with future Redis integration)
- **Authentication**: JWT (JSON Web Tokens) for authentication (to be added in future)
- **Logging**: Serilog (for structured logging)
- **Testing**: xUnit, Moq (for unit and integration tests)
- **Middleware**: Custom error handling middleware for centralized exception management
- **Other Libraries**: Entity Framework Core, FluentValidation (for request validation)

## Requirements

- .NET 5.0 or higher
- PostgreSQL database
- OpenSearch instance (or another search service)
- An API client like Postman for testing

## Installation

### Step 1: Clone the Repository

```bash
git clone https://github.com/hanie21/AutoSphereApi.git
cd AutoSphereApi
```

### Step 2: Set Up the Database

1. Ensure PostgreSQL is installed and running.
2. Create a database for the project.
3. Configure your connection string in `appsettings.json` to point to your PostgreSQL instance.

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=AutoSphereDb;Username=yourusername;Password=yourpassword"
}
```

### Step 3: Apply Migrations

Run the following command to apply migrations to your PostgreSQL database:

```bash
dotnet ef database update
```

### Step 4: Configure OpenSearch (Optional)

If you're using OpenSearch for vehicle search indexing:
1. Set up an OpenSearch instance.
2. Ensure it's running and accessible from your API.
3. Configure OpenSearch settings in your `appsettings.json`:

```json
"OpenSearchSettings": {
  "Url": "http://localhost:9200",
  "IndexName": "vehicles"
}
```

### Step 5: Run the API

Run the API using the following command:

```bash
dotnet run
```
