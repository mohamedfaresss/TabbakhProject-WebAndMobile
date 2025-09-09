# Tabbakh – Backend

Graduation Project – Backend part (ASP.NET Core Web API).

## Features
- Authentication & Authorization (JWT)
- Recipes API with Favorites & Cart
- Multilingual Support (English & Arabic)
- Swagger Documentation

## Tech Stack
ASP.NET Core 8, EF Core, AutoMapper, SQL Server, Docker

## How to Run
1. Clone the repo
2. Update connection string in `appsettings.json`
3. Run:
   ```bash
   dotnet restore
   dotnet ef database update
   dotnet run
