# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копіюємо всі csproj файли відносно кореня рішення
COPY ["Integra-V.WebApi/Integra-V.WebApi.csproj", "Integra-V.WebApi/"]
COPY ["Integra-V.Application/Integra-V.Application.csproj", "Integra-V.Application/"]
COPY ["Integra-V.Domain/Integra-V.Domain.csproj", "Integra-V.Domain/"]
COPY ["Integra-V.Infrastructure/Integra-V.Infrastructure.csproj", "Integra-V.Infrastructure/"]

# Відновлюємо залежності
RUN dotnet restore "Integra-V.WebApi/Integra-V.WebApi.csproj"

# Копіюємо весь код рішення
COPY . .

WORKDIR "/src/Integra-V.WebApi"
RUN dotnet build "Integra-V.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Integra-V.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Integra-V.WebApi.dll"]
