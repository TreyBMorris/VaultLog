FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY backend/src/VaultLog.slnx ./
COPY backend/src/VaultLog.API/VaultLog.API.csproj ./VaultLog.API/
COPY backend/src/VaultLog.Core/VaultLog.Core.csproj ./VaultLog.Core/
COPY backend/src/VaultLog.Infrastructure/VaultLog.Infrastructure.csproj ./VaultLog.Infrastructure/

RUN dotnet restore VaultLog.slnx

COPY backend/src/ .

RUN dotnet publish VaultLog.API/VaultLog.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "VaultLog.API.dll"]
