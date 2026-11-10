# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY . .

RUN dotnet restore "Api/Api.csproj"

WORKDIR /src/Api

RUN dotnet publish "Api.csproj" \
    -c $BUILD_CONFIGURATION \
    --no-restore \
    -o /app/publish \
    /p:UseAppHost=false


# Этап запуска
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

# Системные библиотеки для SkiaSharp
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        libfontconfig1 \
        libfreetype6 \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

EXPOSE 8080

ENV ASPNETCORE_HTTP_PORTS=8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Api.dll"]
