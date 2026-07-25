FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "Gateway/Microservices.ApiGateway/Microservices.ApiGateway.csproj"
RUN dotnet publish "Gateway/Microservices.ApiGateway/Microservices.ApiGateway.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Microservices.ApiGateway.dll"]
