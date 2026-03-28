FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Timetable/Timetable.DbMigrator/Timetable.DbMigrator.csproj", "Timetable/Timetable.DbMigrator/"]
RUN dotnet restore "./Timetable/Timetable.DbMigrator/./Timetable.DbMigrator.csproj"
COPY . .
WORKDIR "/src/Timetable/Timetable.DbMigrator"
RUN dotnet build "./Timetable.DbMigrator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Timetable.DbMigrator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Timetable.DbMigrator.dll"]
