FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "Services/Ordering/Microservices.Ordering.Api/Microservices.Ordering.Api.csproj"
RUN dotnet publish "Services/Ordering/Microservices.Ordering.Api/Microservices.Ordering.Api.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Microservices.Ordering.Api.dll"]
