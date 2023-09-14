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

#### Create the database container (you need to have Docker installed on your system):

```sh
docker compose -f docker-compose-db-postgres.yml up -d
```

#### Create the image
```sh
docker build -t proyecta-webapi-dotnet -f Proyecta.Web/Dockerfile .
```

#### Run containers
```sh
docker compose -f docker-compose-webapi-dev.yml up -d
docker compose -f docker-compose-webapi-test.yml up -d
```

## EF Migrations
#### Add a new migration when needed

```sh
dotnet ef migrations add "AuthInitialMigration" --project Proyecta.Repository.EntityFramework --startup-project Proyecta.Web --context AuthDbContext
dotnet ef migrations add "AppInitialMigration" --project Proyecta.Repository.EntityFramework --startup-project Proyecta.Web --context AppDbContext
```