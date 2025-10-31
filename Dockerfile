# Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Çözüm dosyasını kopyala
COPY TrenRezervasyon.sln ./

# Proje dosyalarını kopyala
COPY api/api.csproj ./api/

# Restore
RUN dotnet restore TrenRezervasyon.sln

# Tüm dosyaları kopyala
COPY . ./

# Publish
RUN dotnet publish api/api.csproj -c Release -o out

# Runtime aşaması
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Yayınlanan dosyaları kopyala
COPY --from=build /app/out .

# Ortam değişkenlerini ayarla
ENV ASPNETCORE_URLS=http://+:${PORT:-5000}
EXPOSE 5000

# Uygulamayı başlat
ENTRYPOINT ["dotnet", "api.dll"]
