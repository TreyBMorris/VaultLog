FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy shared MSBuild props first
COPY backend/Directory.Build.props ./backend/
COPY backend/Directory.Packages.props ./backend/

# Copy project files for layer caching
COPY backend/src/VaultLog.API/VaultLog.API.csproj ./backend/src/VaultLog.API/
COPY backend/src/VaultLog.Core/VaultLog.Core.csproj ./backend/src/VaultLog.Core/
COPY backend/src/VaultLog.Infrastructure/VaultLog.Infrastructure.csproj ./backend/src/VaultLog.Infrastructure/

RUN dotnet restore backend/src/VaultLog.API/VaultLog.API.csproj

# Copy the rest of the source
COPY backend/ ./backend/

RUN dotnet publish backend/src/VaultLog.API/VaultLog.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5050

EXPOSE 5050

ENTRYPOINT ["dotnet", "VaultLog.API.dll"]
