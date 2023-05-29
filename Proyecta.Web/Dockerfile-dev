FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Proyecta.Web/Proyecta.Web.csproj", "Proyecta.Web/"]
COPY ["Proyecta.Core/Proyecta.Core.csproj", "Proyecta.Core/"]
COPY ["Proyecta.Repository.EntityFramework/Proyecta.Repository.EntityFramework.csproj", "Proyecta.Repository.EntityFramework/"]
RUN dotnet restore "Proyecta.Web/Proyecta.Web.csproj"
COPY . .
WORKDIR "/src/Proyecta.Web"
RUN dotnet build "Proyecta.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Proyecta.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Proyecta.Web.dll"]
