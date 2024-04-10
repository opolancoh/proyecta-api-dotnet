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

#### Database Setup
Launch a PostgreSQL container as the database:
```sh
docker run -d --name proyecta_postgres -p 5433:5432 -e POSTGRES_PASSWORD=mysecretpassword --network proyecta-network postgres
```
Note: The port mapping "5433:5432" allows the host machine to communicate with PostgreSQL on its default port (5432) through the host's port 5433.

#### Building the Image
Build a Docker image for the API:
```sh
docker build -t proyecta_api_dotnet:latest -f Proyecta.Web/Dockerfile .
```

#### Running Containers
Start the application and test containers:
```sh
docker compose -f docker-compose-dev.yml up -d
docker compose -f docker-compose-test.yml up -d
```

## Managing EF Migrations
#### Creating New Migrations
To add new migrations, use the following commands:
```sh
dotnet ef migrations add "AuthInitialMigration" --project Proyecta.Repository.EntityFramework --startup-project Proyecta.Web --context AuthDbContext
dotnet ef migrations add "AppInitialMigration" --project Proyecta.Repository.EntityFramework --startup-project Proyecta.Web --context AppDbContext
```