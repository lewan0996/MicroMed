FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Doctors/Doctors.DbMigrator/Doctors.DbMigrator.csproj", "Doctors/Doctors.DbMigrator/"]
RUN dotnet restore "./Doctors/Doctors.DbMigrator/./Doctors.DbMigrator.csproj"
COPY . .
WORKDIR "/src/Doctors/Doctors.DbMigrator"
RUN dotnet build "./Doctors.DbMigrator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Doctors.DbMigrator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Doctors.DbMigrator.dll"]
