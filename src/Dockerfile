# Build stage
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build-env
WORKDIR /app

COPY ./API/API.csproj ./API/
COPY ./Infrastructure/Infrastructure.csproj ./Infrastructure/
COPY ./Domain/Domain.csproj ./Domain/
RUN dotnet restore ./API/API.csproj

COPY . .
RUN dotnet publish ./API/API.csproj -c Release -o /publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /publish
COPY --from=build-env /publish .
ENTRYPOINT ["dotnet", "API.dll"]
