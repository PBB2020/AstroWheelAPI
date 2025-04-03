# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Csak a főprojekt másolása és restore
COPY ["AstroWheelAPI/AstroWheelAPI.csproj", "AstroWheelAPI/"]
RUN dotnet restore "AstroWheelAPI/AstroWheelAPI.csproj"

# 2. Összes fájl másolása (a .dockerignore kizárja a tesztprojektet)
COPY . .

# 3. Publikálás explicit a főprojektre
WORKDIR "/src/AstroWheelAPI"
RUN dotnet publish -c Release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "AstroWheelAPI.dll"]