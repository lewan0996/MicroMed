FROM mcr.microsoft.com/devcontainers/dotnet:10.0

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        postgresql-client \
    && rm -rf /var/lib/apt/lists/*

RUN dotnet tool install --tool-path /usr/local/bin dotnet-ef

ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV DOTNET_NOLOGO=true

WORKDIR /workspace
