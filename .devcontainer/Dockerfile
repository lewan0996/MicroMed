FROM mcr.microsoft.com/dotnet/sdk:10.0 AS dotnet_tools

RUN dotnet tool install --global dotnet-ef

FROM mcr.microsoft.com/dotnet/sdk:10.0

RUN apt-get update && apt-get install -y --no-install-recommends postgresql-client && rm -rf /var/lib/apt/lists/*

WORKDIR /workspace

COPY --from=dotnet_tools /root/.dotnet/tools /root/.dotnet/tools

ENV PATH="$PATH:/root/.dotnet/tools"