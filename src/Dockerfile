FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build-env
WORKDIR /app

# Copy everything and build the project
COPY . ./

RUN dotnet restore ./API/API.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish ./API/API.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "API.dll"]

