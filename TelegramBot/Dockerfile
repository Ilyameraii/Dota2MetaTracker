# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Копируем всё решение, включая все проекты и их зависимости
COPY . .

# Восстанавливаем NuGet-зависимости
RUN dotnet restore "TelegramBot/TelegramBot.csproj"

# Публикуем приложение
WORKDIR /src/TelegramBot

RUN dotnet publish "TelegramBot.csproj" \
    -c $BUILD_CONFIGURATION \
    --no-restore \
    -o /app/publish \
    /p:UseAppHost=false


# Этап запуска
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

# Устанавливаем системные библиотеки для SkiaSharp
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        libfontconfig1 \
        libfreetype6 \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

EXPOSE 8080

ENV ASPNETCORE_HTTP_PORTS=8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "TelegramBot.dll"]