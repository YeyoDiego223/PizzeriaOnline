# Usa la imagen oficial de .NET 8 como base para construir
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia los archivos del proyecto y restaura las dependencias
COPY *.csproj ./
RUN dotnet restore

# Copia el resto del código de la aplicación
COPY . ./
WORKDIR /app
RUN dotnet publish -c Release -o /app/publish

# Usa la imagen más ligera de ASP.NET para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PizzeriaOnline.dll"]