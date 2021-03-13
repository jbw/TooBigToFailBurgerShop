#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["services/Ordering/Ordering.API/Ordering.API.csproj", "services/Ordering/Ordering.API/"]
COPY ["services/Ordering/Ordering.Domain.Core/Ordering.Domain.Core.csproj", "services/Ordering/Ordering.Domain.Core/"]
COPY ["services/Ordering/Ordering.Persistence.RabbitMQ/Ordering.Persistence.MassTransit.csproj", "services/Ordering/Ordering.Persistence.RabbitMQ/"]
COPY ["services/Ordering/Ordering.Messages/Ordering.Contracts.csproj", "services/Ordering/Ordering.Messages/"]
COPY ["services/Ordering/Ordering.Domain/Ordering.Domain.csproj", "services/Ordering/Ordering.Domain/"]
COPY ["services/Ordering/Ordering.Persistence.MartenDb/Ordering.Persistence.MartenDb.csproj", "services/Ordering/Ordering.Persistence.MartenDb/"]
COPY ["services/Ordering/Ordering.Persistence.Mongo/Ordering.Persistence.Mongo.csproj", "services/Ordering/Ordering.Persistence.Mongo/"]
COPY ["services/Ordering/Ordering.Application/Ordering.Application.csproj", "services/Ordering/Ordering.Application/"]
COPY ["services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj", "services/Ordering/Ordering.Infrastructure/"]
RUN dotnet restore "services/Ordering/Ordering.API/Ordering.API.csproj"
COPY . .
WORKDIR "/src/services/Ordering/Ordering.API"
RUN dotnet build "Ordering.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ordering.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.API.dll"]