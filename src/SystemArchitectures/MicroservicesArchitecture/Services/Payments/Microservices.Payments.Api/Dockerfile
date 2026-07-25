FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "Services/Payments/Microservices.Payments.Api/Microservices.Payments.Api.csproj"
RUN dotnet publish "Services/Payments/Microservices.Payments.Api/Microservices.Payments.Api.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Microservices.Payments.Api.dll"]
