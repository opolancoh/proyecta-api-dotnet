# Proyecta backend
A .NET Core web api.

#### Technologies in this repo:
* DotNet Core 7
* Entity Framework 7
* Postgres (Docker Container)
* xUnit (Integration Tests)

## Docker Containers (execute the commands in the root folder)
#### Create the network
```sh
docker network create proyecta-network 
```

#### Create the database:

```sh
docker run -d --name proyecta_postgres -p 5433:5432 -e POSTGRES_PASSWORD=mysecretpassword --network proyecta-network postgres
```
* "5433:5432" maps port 5433 on the host to port 5432 in the container, which is the default port for PostgreSQL. This allows the PostgreSQL service to be accessible from the host machine.

#### Create the image
```sh
docker build -t proyecta_api_dotnet:latest -f Proyecta.Web/Dockerfile .
```

#### Run containers
```sh
docker compose -f docker-compose-api-dev.yml up -d
docker compose -f docker-compose-api-test.yml up -d
```

## EF Migrations
#### Add a new migration when needed
```sh
dotnet ef migrations add "AuthInitialMigration" --project Proyecta.Repository.EntityFramework --startup-project Proyecta.Web --context AuthDbContext
dotnet ef migrations add "AppInitialMigration" --project Proyecta.Repository.EntityFramework --startup-project Proyecta.Web --context AppDbContext
```