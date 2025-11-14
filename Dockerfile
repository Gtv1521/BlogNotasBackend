# PASE A PRODUCCION 

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /src
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./BackEndNotes.csproj"
RUN dotnet build "./BackEndNotes.csproj" -c Release -o /src/build

FROM build AS publish
RUN dotnet publish "./BackEndNotes.csproj" -c Release -o /src/publish

FROM base AS final
WORKDIR /src
COPY --from=publish /src/publish .
ENTRYPOINT ["dotnet", "BackEndNotes.dll"]