# Proyecta API
This repository contains a .NET Core Web API.

### Technologies Used:
* DotNet Core 7
* Entity Framework 7
* Postgres (Docker Container)
* xUnit (Integration Tests)

## Setting Up Docker Containers (Run these commands from the root directory)
### Network Setup
First, create a dedicated network for the containers:
```sh
docker network create proyecta-network
```

### Database Setup
Launch a PostgreSQL container as the database:
```sh
docker run -d --name proyecta_db_postgres -p 5432:5432 -e POSTGRES_PASSWORD=mysecretpassword --network proyecta-network postgres:16.3
```
Note: The port mapping "5433:5432" allows the host machine to communicate with PostgreSQL on its default port (5432) through the host's port 5433.

### Building the Image
Build a Docker image for the API:
```sh
docker build -t proyecta/api-dotnet:latest -f Proyecta.Web/Dockerfile .
```

### Running Containers
Start the application and test the containers. If environment variables are not specified, the default port values will be HTTP_PORT=5000 and HTTPS_PORT=5001.
```sh
export HTTP_PORT=5100
export HTTPS_PORT=5101
docker compose -f docker-compose-dev.yml up -d
```

## Managing EF Migrations
#### Creating New Migrations
To add new migrations, use the following commands:
```sh
 dotnet ef migrations add "AuthInitialMigration" --project Proyecta.Repository.EntityFramework --startup-project Proyecta.Web --context AuthDbContext --output-dir Migrations/AuthDb
 dotnet ef migrations add "ApiInitialMigration" --project Proyecta.Repository.EntityFramework --startup-project Proyecta.Web --context ApiDbContext --output-dir Migrations/ApiDb
```

## API Documentation

To access the Swagger UI, navigate to /api-docs
