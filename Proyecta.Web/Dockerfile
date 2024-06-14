# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy the .csproj files first to restore dependencies
COPY ["Proyecta.Web/Proyecta.Web.csproj", "Proyecta.Web/"]
COPY ["Proyecta.Core/Proyecta.Core.csproj", "Proyecta.Core/"]
COPY ["Proyecta.Services/Proyecta.Services.csproj", "Proyecta.Services/"]
COPY ["Proyecta.Repository.EntityFramework/Proyecta.Repository.EntityFramework.csproj", "Proyecta.Repository.EntityFramework/"]
RUN dotnet restore "Proyecta.Web/Proyecta.Web.csproj"

# Copy the rest of the source code and publish the API project
COPY . .
RUN dotnet publish "Proyecta.Web/Proyecta.Web.csproj" -c Release -o /app/out

# Generate runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Update package index, install necessary packages, and clean up
RUN apt-get update && apt-get install -y --no-install-recommends iputils-ping && rm -rf /var/lib/apt/lists/*

# Copy the built application from the build stage
COPY --from=build /app/out .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "Proyecta.Web.dll"]
