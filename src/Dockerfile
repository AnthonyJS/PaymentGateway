FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build-env

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

# Copy everything and build the project
COPY . ./

RUN dotnet restore ./API/API.csproj

COPY . ./build/
WORKDIR /build/
RUN dotnet publish ./API/API.csproj -c $BUILDCONFIG -o out /p:Version=$VERSION

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
# FROM mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /app

COPY --from=build-env /build/out .

ENTRYPOINT ["dotnet", "API.dll"]
