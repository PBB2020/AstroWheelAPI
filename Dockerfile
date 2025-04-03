# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Főprojekt és tesztprojekt fájljainak másolása
COPY ["AstroWheelAPI/AstroWheelAPI.csproj", "AstroWheelAPI/"]
COPY ["Test.AstroWheelApi/Test.AstroWheelApi.csproj", "Test.AstroWheelApi/"]
RUN dotnet restore "AstroWheelAPI/AstroWheelAPI.csproj"  # Csak a főprojekt restore

# 2. Összes fájl másolása (beleértve a tesztprojektet)
COPY . .

# 3. Publikálás explicit a főprojektre (a tesztprojekt nem kerül bele)
WORKDIR "/src/AstroWheelAPI"
RUN dotnet publish -c Release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "AstroWheelAPI.dll"]