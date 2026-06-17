FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Dota2MetaTracker/Dota2MetaTracker.csproj", "Dota2MetaTracker/"]
RUN dotnet restore "Dota2MetaTracker/Dota2MetaTracker.csproj"
COPY . .
WORKDIR "/src/Dota2MetaTracker"
RUN dotnet build "./Dota2MetaTracker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Dota2MetaTracker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dota2MetaTracker.dll"]
