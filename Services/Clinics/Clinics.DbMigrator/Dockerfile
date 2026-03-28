FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Clinics/Clinics.DbMigrator/Clinics.DbMigrator.csproj", "Clinics/Clinics.DbMigrator/"]
RUN dotnet restore "./Clinics/Clinics.DbMigrator/./Clinics.DbMigrator.csproj"
COPY . .
WORKDIR "/src/Clinics/Clinics.DbMigrator"
RUN dotnet build "./Clinics.DbMigrator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Clinics.DbMigrator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Clinics.DbMigrator.dll"]
