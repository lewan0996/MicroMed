FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src
COPY . .

WORKDIR /src/Clinics/Clinics.API

RUN dotnet publish \
    -c Release \
    -o /app \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0

USER app
WORKDIR /app

COPY --from=build /app .

ENTRYPOINT ["dotnet", "Clinics.API.dll"]