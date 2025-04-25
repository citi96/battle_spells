# ---------- BUILD (SDK) ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src

COPY . .
RUN dotnet publish "Battle Spells.Api/Battle Spells.Api.csproj" \
    -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

RUN dotnet tool install -g dotnet-ef   # EF CLI per le migration

# ---------- RUNTIME (ASP.NET + LiteFS) ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
ENV ASPNETCORE_URLS=http://*:8080
WORKDIR /app

RUN apt-get update -y && \
    apt-get install -y ca-certificates fuse3 sqlite3 && \
    rm -rf /var/lib/apt/lists/*

COPY --from=flyio/litefs:0.5 /usr/local/bin/litefs /usr/local/bin/litefs
RUN chmod +x /usr/local/bin/litefs

# Copia tool globali (dotnet-ef) dallo stage build
COPY --from=build /root/.dotnet /root/.dotnet
ENV PATH="$PATH:/root/.dotnet/tools"

# App pubblicata e configurazione LiteFS
COPY --from=build /app/publish/ .
COPY litefs.yml /etc/litefs.yml

VOLUME ["/var/lib/litefs"]

ENTRYPOINT ["litefs", "mount", "-config", "/etc/litefs.yml"]

EXPOSE 8080
