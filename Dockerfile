FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build-dotnet
RUN apt-get update && \
    apt-get install -y curl && \
    curl -fsSL https://deb.nodesource.com/setup_18.x | bash - && \
    apt-get install -y nodejs && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY ["src/Host/YAGO.World.Host.csproj", "Host/"]
RUN dotnet restore "./Host/YAGO.World.Host.csproj"
COPY src/. .
WORKDIR /app/Host/ClientApp
RUN npm install
RUN npm run build
WORKDIR /app/Host
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final
WORKDIR /app
COPY --from=build-dotnet /app/Host/out ./
COPY --from=build-dotnet /app/Host/ClientApp/dist ./ClientApp/dist
ENTRYPOINT ["dotnet", "YAGO.World.Host.dll"]