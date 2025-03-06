# FROM mcr.microsoft.com/dotnet/sdk:8.0.406 AS build
# WORKDIR /src
# COPY *.csproj .
# RUN dotnet restore
# COPY . .
# RUN dotnet publish -c Release -o /src

# # RUN dotnet whit version 9
# FROM mcr.microsoft.com/dotnet/aspnet:8.0.406 AS final
# WORKDIR /app
# COPY --from=build /app .

# EXPOSE 5200
# ENTRYPOINT ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:5200"]

# Usar la imagen base de SDK de .NET 8 para el desarrollo
FROM mcr.microsoft.com/dotnet/sdk:8.0.406 AS build

WORKDIR /src

# Copiar el archivo de proyecto y el archivo global.json
COPY *.csproj .

RUN dotnet restore
COPY . .

# Ejecutar la aplicación
EXPOSE 5217
ENTRYPOINT ["dotnet", "watch", "run"]