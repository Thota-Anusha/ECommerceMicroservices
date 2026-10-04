# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY PaymentService.API/PaymentService.API.csproj PaymentService.API/
COPY PaymentService.Business/PaymentService.Business.csproj PaymentService.Business/
COPY PaymentService.Data/PaymentService.Data.csproj PaymentService.Data/
COPY PaymentService.Models/PaymentService.Models.csproj PaymentService.Models/

RUN dotnet restore PaymentService.API/PaymentService.API.csproj

COPY PaymentService.API/ PaymentService.API/
COPY PaymentService.Business/ PaymentService.Business/
COPY PaymentService.Data/ PaymentService.Data/
COPY PaymentService.Models/ PaymentService.Models/

RUN dotnet build PaymentService.API/PaymentService.API.csproj -c Release -o /app/build

RUN dotnet publish PaymentService.API/PaymentService.API.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "PaymentService.API.dll"]