# Utiliza la imagen oficial de SDK de .NET Core 8.0 como imagen base.
FROM mcr.microsoft.com/dotnet/core/sdk:8.0 AS build

# Establece el directorio de trabajo dentro del contenedor.
WORKDIR /app

# Copia todos los archivos *.csproj y el código fuente en /app/src/.
COPY *.csproj ./
COPY ./src/*.cs ./src/

# Instala las dependencias NuGet.
RUN dotnet restore --no-cache

# Compila la aplicación con la configuración de producción.
RUN dotnet build --configuration Release -c src

# Copia los archivos de salida en el directorio de trabajo.
COPY ./src/obj/Release/*.dll ./

# Establece la entrada al aplicativo como punto de partida para el contenedor.
ENTRYPOINT ["dotnet", "run"]

# Exposición del puerto 80 para que la aplicación esté disponible desde fuera del contenedor.
EXPOSE 80

# Comenta esta línea si no deseas ejecutar automáticamente el comando CMD.
CMD ["dotnet", "run"]