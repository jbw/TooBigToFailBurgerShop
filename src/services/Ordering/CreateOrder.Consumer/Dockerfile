#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["services/Ordering/CreateOrder.Consumer/CreateOrder.Consumer.csproj", "services/Ordering/CreateOrder.Consumer/"]
COPY ["services/Ordering/Ordering.Domain.Core/Ordering.Domain.Core.csproj", "services/Ordering/Ordering.Domain.Core/"]
COPY ["services/Ordering/Ordering.Persistence.RabbitMQ/Ordering.Persistence.MassTransit.csproj", "services/Ordering/Ordering.Persistence.RabbitMQ/"]
COPY ["services/Ordering/Ordering.Messages/Ordering.Contracts.csproj", "services/Ordering/Ordering.Messages/"]
COPY ["services/Ordering/Ordering.Domain/Ordering.Domain.csproj", "services/Ordering/Ordering.Domain/"]
COPY ["services/Ordering/Ordering.Persistence.MartenDb/Ordering.Persistence.MartenDb.csproj", "services/Ordering/Ordering.Persistence.MartenDb/"]
COPY ["services/Ordering/Ordering.Persistence.Mongo/Ordering.Persistence.Mongo.csproj", "services/Ordering/Ordering.Persistence.Mongo/"]
COPY ["services/Ordering/Ordering.Application/Ordering.Application.csproj", "services/Ordering/Ordering.Application/"]
COPY ["services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj", "services/Ordering/Ordering.Infrastructure/"]
RUN dotnet restore "services/Ordering/CreateOrder.Consumer/CreateOrder.Consumer.csproj"
COPY . .
WORKDIR "/src/services/Ordering/CreateOrder.Consumer"
RUN dotnet build "CreateOrder.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CreateOrder.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CreateOrder.Consumer.dll"]