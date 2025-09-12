# Etapa 1: Construir la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia el archivo de solución (.sln) y los archivos de proyecto (.csproj)
# respetando la estructura de carpetas.
COPY ["PizzeriaOnline.sln", "."]
COPY ["PizzeriaOnline/PizzeriaOnline.csproj", "PizzeriaOnline/"]

# Ejecuta 'dotnet restore' sobre el archivo de solución
RUN dotnet restore "PizzeriaOnline.sln"

# Copia todo el resto del código fuente
COPY . .

# Mueve a la carpeta del proyecto y publica la aplicación
WORKDIR "/src/PizzeriaOnline"
RUN dotnet publish "PizzeriaOnline.csproj" -c Release -o /app/publish

# Etapa 2: Crear la imagen final de ejecución (más ligera)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PizzeriaOnline.dll"]