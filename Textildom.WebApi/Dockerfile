	# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

	# This stage is used when running from VS in fast mode (Default for Debug configuration)
	FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
	USER $APP_UID
	WORKDIR /app
	EXPOSE 8080
	EXPOSE 8081


	# This stage is used to build the service project
	FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
	ARG BUILD_CONFIGURATION=Release
	WORKDIR /src
	COPY ["Textildom.WebApi/Textildom.WebApi.csproj", "Textildom.WebApi/"]
	COPY ["Textildom.Application/Textildom.Application.csproj", "Textildom.Application/"]
	COPY ["Textildom.Domain/Textildom.Domain.csproj", "Textildom.Domain/"]
	COPY ["Textildom.Infrastructure/Textildom.Infrastructure.csproj", "Textildom.Infrastructure/"]
	RUN dotnet restore "./Textildom.WebApi/Textildom.WebApi.csproj"
	COPY . .
	WORKDIR "/src/Textildom.WebApi"
	RUN dotnet build "./Textildom.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

	# This stage is used to publish the service project to be copied to the final stage
	FROM build AS publish
	ARG BUILD_CONFIGURATION=Release
	RUN dotnet publish "./Textildom.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

	# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
	FROM base AS final
	WORKDIR /app
	COPY --from=publish /app/publish .

	USER root

	RUN mkdir -p /app/wwwroot/uploads/products

	RUN chmod -R 777 /app/wwwroot

	ENTRYPOINT ["dotnet", "Textildom.WebApi.dll"]