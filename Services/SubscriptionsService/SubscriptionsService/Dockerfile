FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

RUN addgroup --gid 1000 appgroup && \
    adduser --disabled-password --uid 1000 --gid 1000 appuser

USER appuser
WORKDIR /app
EXPOSE 8016

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/SubscriptionsService/SubscriptionsService/SubscriptionsService.csproj", "Services/SubscriptionsService/SubscriptionsService/"]
RUN dotnet restore "Services/SubscriptionsService/SubscriptionsService/SubscriptionsService.csproj"
COPY . .
WORKDIR "/src/Services/SubscriptionsService/SubscriptionsService"
RUN dotnet build "SubscriptionsService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SubscriptionsService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SubscriptionsService.dll"]