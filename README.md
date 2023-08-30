# Proyecta backend
A .NET Core web api.

#### Technologies in this repo:
* DotNet Core 7
* Entity Framework 7
* Postgres (Docker Container)
* xUnit (Integration Tests)

#### Setup Database
Create the database container (you need to have Docker installed on your system):

```sh
docker compose -f "docker-compose-db-postgres.yml" up -d
```

Stop and remove the container when needed:

```sh
docker stop proyecta-db_postgres && docker rm proyecta-db_postgres
```

#### Create Database

```sh
dotnet ef database update --project Proyecta.Repository.EntityFramework --startup-project Proyecta.Web --context AuthDbContext
dotnet ef database update --project Proyecta.Repository.EntityFramework --startup-project Proyecta.Web --context AppDbContext
```

#### Add a new migration when needed

```sh
dotnet ef migrations add "AuthInitialMigration" --project Proyecta.Repository.EntityFramework --startup-project Proyecta.Web --context AuthDbContext
dotnet ef migrations add "AppInitialMigration" --project Proyecta.Repository.EntityFramework --startup-project Proyecta.Web --context AppDbContext
```

#### Docker Containers (execute the commands in the root folder)
Create the image:
```sh
docker build -t proyecta-backend-dotnet -f Proyecta.Web/Dockerfile .
```

Run containers:
```sh
docker compose -f "docker-compose-backend-dev.yml" up -d
docker compose -f "docker-compose-backend-test.yml" up -d
```