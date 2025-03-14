FROM mcr.microsoft.com/dotnet/sdk:8.0.406 AS build

WORKDIR /src

# Copiar el archivo de proyecto y el archivo global.json
COPY *.csproj .

RUN dotnet restore
COPY . .

# Ejecutar la aplicación
EXPOSE 5217
ENTRYPOINT ["dotnet", "watch", "run"]