FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-dotnet

# Устанавливаем Node.js 20
RUN apt-get update && \
    apt-get install -y curl xz-utils && \
    curl -fsSL https://nodejs.org/dist/v20.19.5/node-v20.19.5-linux-x64.tar.xz | tar -xJ -C /usr/local --strip-components=1 && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Копируем и восстанавливаем .NET зависимости
COPY ["src/Host/YAGO.World.Host.csproj", "Host/"]
RUN dotnet restore "./Host/YAGO.World.Host.csproj"

# Копируем исходники .NET
COPY src/. .

# --- FRONTEND BUILD (с очисткой) ---
WORKDIR /app/Host/ClientApp

# Копируем package файлы отдельно (для кэширования)
COPY src/Host/ClientApp/package*.json ./

# Устанавливаем зависимости с очисткой
RUN rm -rf node_modules package-lock.json && \
    npm cache clean --force && \
    npm install --no-cache

# Копируем фронтенд исходники
COPY src/Host/ClientApp/ .

# Сборка фронтенда
RUN npm run build

# --- BACKEND BUILD ---
WORKDIR /app/Host
RUN dotnet publish -c Release -o out

# --- FINAL IMAGE ---
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build-dotnet /app/Host/out ./
COPY --from=build-dotnet /app/Host/ClientApp/dist ./ClientApp/dist

ENTRYPOINT ["dotnet", "YAGO.World.Host.dll"]