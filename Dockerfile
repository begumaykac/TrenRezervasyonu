# 1. Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Proje dosyalarını kopyala
COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

# 2. Runtime aşaması
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Portu tanımla (Render PORT environment variable ile yönlendirir)
ENV ASPNETCORE_URLS=http://+:${PORT:-5000}
EXPOSE 5000

# Çalıştır
ENTRYPOINT ["dotnet", "TrenRezervasyon.dll"]
