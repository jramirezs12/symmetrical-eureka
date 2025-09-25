#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RulesEngine/RulesEngineAPI.csproj", "RulesEngine/"]
COPY ["RulesEngine.Application/RulesEngine.Application.csproj", "RulesEngine.Application/"]
COPY ["RulesEngine.Domain/RulesEngine.Domain.csproj", "RulesEngine.Domain/"]
COPY ["RulesEngineContracts/RulesEngineContracts.csproj", "RulesEngineContracts/"]
COPY ["RulesEngine.Infrastructure/RulesEngine.Infrastructure.csproj", "RulesEngine.Infrastructure/"]
COPY ["RulesEngine.Persistence/RulesEngine.Persistence.csproj", "RulesEngine.Persistence/"]
RUN dotnet restore "./RulesEngine/RulesEngineAPI.csproj"
COPY . .
WORKDIR "/src/RulesEngine"
RUN dotnet build "./RulesEngineAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

ENV GiproDatabases__ConnectionString="cadena de conexion" \
    GiproDatabases__DatabaseNameCollection__Mundial="" \
    Serilog__WriteTo__1__Args__serverUrl="server de seq" \
    BasicGiproDatabases__ConnectionString="Cadena de conexion BD basicas" \
    BasicGiproDatabases__DatabaseNameCollection__Mundial="Nombre BD basicas"

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RulesEngineAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RulesEngineAPI.dll"]