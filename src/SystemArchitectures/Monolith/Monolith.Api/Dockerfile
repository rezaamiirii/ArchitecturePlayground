FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/SystemArchitectures/Monolith/Monolith.Api/Monolith.Api.csproj src/SystemArchitectures/Monolith/Monolith.Api/
RUN dotnet restore src/SystemArchitectures/Monolith/Monolith.Api/Monolith.Api.csproj
COPY src/SystemArchitectures/Monolith/Monolith.Api/ src/SystemArchitectures/Monolith/Monolith.Api/
RUN dotnet publish src/SystemArchitectures/Monolith/Monolith.Api/Monolith.Api.csproj -c Release -o /app/publish --no-restore

FROM base AS final
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Monolith.Api.dll"]
