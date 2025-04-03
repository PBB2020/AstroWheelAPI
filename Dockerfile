# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Összes fájl másolása
COPY . .

# 2. Csak a főprojekt másolása és restore
COPY ["AstroWheelAPI/AstroWheelAPI.csproj", "./"]
RUN dotnet restore "AstroWheelAPI.csproj"

# 3. Publikálás explicit a főprojektre
WORKDIR "/src/AstroWheelAPI"
RUN dotnet publish -c Release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "AstroWheelAPI.dll"]