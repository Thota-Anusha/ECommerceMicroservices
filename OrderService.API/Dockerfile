# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy project files
COPY OrderService.API/OrderService.API.csproj OrderService.API/
COPY OrderService.Business/OrderService.Business.csproj OrderService.Business/
COPY OrderService.Data/OrderService.Data.csproj OrderService.Data/
COPY OrderService.Models/OrderService.Models.csproj OrderService.Models/

# Restore dependencies
RUN dotnet restore OrderService.API/OrderService.API.csproj

# Copy source code
COPY OrderService.API/ OrderService.API/
COPY OrderService.Business/ OrderService.Business/
COPY OrderService.Data/ OrderService.Data/
COPY OrderService.Models/ OrderService.Models/

# Build
RUN dotnet build OrderService.API/OrderService.API.csproj -c Release -o /app/build

# Publish
RUN dotnet publish OrderService.API/OrderService.API.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "OrderService.API.dll"]