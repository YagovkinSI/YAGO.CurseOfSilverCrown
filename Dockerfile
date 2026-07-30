FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-dotnet

# Устанавливаем Node.js 20
RUN apt-get update && \
    apt-get install -y curl xz-utils && \
    curl -fsSL https://nodejs.org/dist/v20.19.5/node-v20.19.5-linux-x64.tar.xz | tar -xJ -C /usr/local --strip-components=1 && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Копируем исходники
COPY src/ ./

# Восстанавливаем зависимости
RUN dotnet restore "Host/YAGO.World.Host.csproj"

# --- FRONTEND BUILD ---
WORKDIR /app/Host/ClientApp

RUN rm -rf node_modules package-lock.json && \
    npm cache clean --force && \
    npm install && \
    npm run build

# Проверяем, что фронт собрался
RUN ls -la dist && test -f dist/index.html || (echo "index.html not found" && exit 1)

# --- BACKEND BUILD ---
WORKDIR /app
RUN dotnet publish "Host/YAGO.World.Host.csproj" -c Release -o out /p:PublishRunVite=false

# --- FINAL IMAGE ---
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Копируем бэкенд
COPY --from=build-dotnet /app/out ./

# ✅ Копируем фронт в папку, которую ожидает SPA middleware (SourcePath = "ClientApp")
COPY --from=build-dotnet /app/Host/ClientApp/dist ./ClientApp/dist

ENTRYPOINT ["dotnet", "YAGO.World.Host.dll"]