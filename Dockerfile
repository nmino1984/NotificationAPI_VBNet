# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build

WORKDIR /src

# Copy project files for layer-cached restore
COPY ["NotificationAPI.Domain/NotificationAPI.Domain.vbproj", "NotificationAPI.Domain/"]
COPY ["NotificationAPI.Application/NotificationAPI.Application.vbproj", "NotificationAPI.Application/"]
COPY ["NotificationAPI.Infrastructure/NotificationAPI.Infrastructure.csproj", "NotificationAPI.Infrastructure/"]
COPY ["NotificationAPI.API/NotificationAPI.API.csproj", "NotificationAPI.API/"]

# Restore only API and its dependencies (excludes Tests project)
RUN dotnet restore "NotificationAPI.API/NotificationAPI.API.csproj"

# Copy all source code
COPY . .

# Build API
RUN dotnet build "NotificationAPI.API/NotificationAPI.API.csproj" -c Release --no-restore

# Publish API layer
RUN dotnet publish "NotificationAPI.API/NotificationAPI.API.csproj" \
    -c Release \
    -o /app/publish \
    --no-build

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine

WORKDIR /app

# Copy published files from build stage
COPY --from=build /app/publish .

# Production environment — Railway/nginx handle HTTPS termination at proxy level
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5000

EXPOSE 5000

# Health check against the actual /health endpoint
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD wget --no-verbose --tries=1 --spider http://localhost:5000/health || exit 1

ENTRYPOINT ["dotnet", "NotificationAPI.API.dll"]
