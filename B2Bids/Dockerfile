# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["B2Bids/B2Bids.APIs.csproj", "B2Bids/"]
COPY ["B2Bids.Identity/Identity.Application.csproj", "B2Bids.Identity/"]
COPY ["Identity.Domain/Identity.Domain.csproj", "Identity.Domain/"]
COPY ["Kernel/Kernel.csproj", "Kernel/"]
COPY ["Identity.Infrastructure/Identity.Infrastructure.csproj", "Identity.Infrastructure/"]
COPY ["MediatorCoordinator/MediatorCoordinator.csproj", "MediatorCoordinator/"]
COPY ["Persistence/Persistence.csproj", "Persistence/"]
COPY ["Bid/Bid.Application.csproj", "Bid/"]
COPY ["Bid.Domain/Bid.Domain.csproj", "Bid.Domain/"]
COPY ["Bid.Infrastructure/Bid.Infrastructure.csproj", "Bid.Infrastructure/"]
COPY ["Notification/Notification.Application.csproj", "Notification/"]
COPY ["Products.Domain/Products.Domain.csproj", "Products.Domain/"]
COPY ["Products.Infrastructure/Products.Infrastructure.csproj", "Products.Infrastructure/"]
COPY ["Products/Products.Application.csproj", "Products/"]
COPY ["Stores.Application/Stores.Application.csproj", "Stores.Application/"]
COPY ["Stores.Domain/Stores.Domain.csproj", "Stores.Domain/"]
COPY ["Stores.Infrastructure/Stores.Infrastructure.csproj", "Stores.Infrastructure/"]
RUN dotnet restore "./B2Bids/B2Bids.APIs.csproj"
COPY . .
WORKDIR "/src/B2Bids"
RUN dotnet build "./B2Bids.APIs.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./B2Bids.APIs.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "B2Bids.APIs.dll"]