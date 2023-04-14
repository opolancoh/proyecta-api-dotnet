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
docker run -d --name my-postgres -p 5432:5432 -e POSTGRES_PASSWORD=My@Passw0rd postgres
```

Stop and remove the container when needed:

```sh
docker stop my-postgres && docker rm my-postgres
```

#### Create Database

```sh
dotnet ef database update --project Proyecta.Repository.EntityFramework --startup-project Proyecta.Web
```

#### Add a new migration when needed

```sh
dotnet ef migrations add "MyMigration" --project Proyecta.Repository.EntityFramework --startup-project Proyecta.Web
```
