FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-dotnet

# Устанавливаем Node.js 20
RUN apt-get update && \
    apt-get install -y curl xz-utils && \
    curl -fsSL https://nodejs.org/dist/v20.19.5/node-v20.19.5-linux-x64.tar.xz | tar -xJ -C /usr/local --strip-components=1 && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Копируем .csproj файлы (для кэширования восстановления)
COPY ["src/Host/YAGO.World.Host.csproj", "Host/"]
COPY ["src/Domain/YAGO.World.Domain.csproj", "Domain/"]
COPY ["src/Application/YAGO.World.Application.csproj", "Application/"]
COPY ["src/Infrastructure/YAGO.World.Infrastructure.csproj", "Infrastructure/"]

# Восстанавливаем зависимости
RUN dotnet restore

# Копируем все исходники
COPY src/. .

# --- FRONTEND BUILD ---
WORKDIR /app/Host/ClientApp

# Устанавливаем и собираем фронт
RUN rm -rf node_modules package-lock.json && \
    npm cache clean --force && \
    npm install && \
    npm run build

# Проверяем, что фронт собрался
RUN test -d dist || (echo "Frontend build failed" && exit 1)

# --- BACKEND BUILD ---
WORKDIR /app/Host
RUN dotnet publish -c Release -o out /p:PublishRunVite=false

# --- FINAL IMAGE ---
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build-dotnet /app/Host/out ./
COPY --from=build-dotnet /app/Host/ClientApp/dist ./wwwroot/dist

ENTRYPOINT ["dotnet", "YAGO.World.Host.dll"]