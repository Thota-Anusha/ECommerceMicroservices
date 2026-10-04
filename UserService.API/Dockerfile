# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy project files
COPY UserService.API/UserService.API.csproj UserService.API/
COPY UserService.Business/UserService.Business.csproj UserService.Business/
COPY UserService.Data/UserService.Data.csproj UserService.Data/
COPY UserService.Models/UserService.Models.csproj UserService.Models/

# Restore dependencies
RUN dotnet restore UserService.API/UserService.API.csproj

# Copy source code
COPY UserService.API/ UserService.API/
COPY UserService.Business/ UserService.Business/
COPY UserService.Data/ UserService.Data/
COPY UserService.Models/ UserService.Models/

# Build
RUN dotnet build UserService.API/UserService.API.csproj -c Release -o /app/build

# Publish
RUN dotnet publish UserService.API/UserService.API.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "UserService.API.dll"]