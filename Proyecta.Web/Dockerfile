# Use the SDK image to build the app
# This is the build stage where we use a full SDK image to restore and build the project
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy the .csproj and restore any dependencies (via dotnet restore)
# Copy only the .csproj initially for better caching and faster build
COPY ["Proyecta.Web/Proyecta.Web.csproj", "Proyecta.Web/"]
COPY ["Proyecta.Core/Proyecta.Core.csproj", "Proyecta.Core/"]
COPY ["Proyecta.Services/Proyecta.Services.csproj", "Proyecta.Services/"]
COPY ["Proyecta.Repository.EntityFramework/Proyecta.Repository.EntityFramework.csproj", "Proyecta.Repository.EntityFramework/"]
RUN dotnet restore "Proyecta.Web/Proyecta.Web.csproj"

# Copy the remaining files and build
# Copy source code and publish the API project
# Assuming it depends on the other projects, they will also be built
COPY . .
RUN dotnet publish Proyecta.Web/Proyecta.Web.csproj -c Release -o /app/out


# Generate runtime image
# This is the runtime stage where we use a runtime-only image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Copy the build app from the build stage and set the entry point of the application
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Proyecta.Web.dll"]
