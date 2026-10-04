# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy project files
COPY ProductService.API/ProductService.API.csproj ProductService.API/
COPY ProductService.Business/ProductService.Business.csproj ProductService.Business/
COPY ProductService.Data/ProductService.Data.csproj ProductService.Data/
COPY ProductService.Models/ProductService.Models.csproj ProductService.Models/

# Restore dependencies
RUN dotnet restore ProductService.API/ProductService.API.csproj

# Copy source code
COPY ProductService.API/ ProductService.API/
COPY ProductService.Business/ ProductService.Business/
COPY ProductService.Data/ ProductService.Data/
COPY ProductService.Models/ ProductService.Models/

# Build
RUN dotnet build ProductService.API/ProductService.API.csproj -c Release -o /app/build

# Publish
RUN dotnet publish ProductService.API/ProductService.API.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "ProductService.API.dll"]