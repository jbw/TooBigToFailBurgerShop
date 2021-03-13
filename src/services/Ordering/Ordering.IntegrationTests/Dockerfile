#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["services/Ordering/Ordering.IntegrationTests/Ordering.IntegrationTests.csproj", "services/Ordering/Ordering.IntegrationTests/"]
COPY ["services/Ordering/Ordering.API/Ordering.API.csproj", "services/Ordering/Ordering.API/"]
COPY ["services/Ordering/Ordering.Domain.Core/Ordering.Domain.Core.csproj", "services/Ordering/Ordering.Domain.Core/"]
COPY ["services/Ordering/Ordering.Persistence.RabbitMQ/Ordering.Persistence.MassTransit.csproj", "services/Ordering/Ordering.Persistence.RabbitMQ/"]
COPY ["services/Ordering/Ordering.Messages/Ordering.Contracts.csproj", "services/Ordering/Ordering.Messages/"]
COPY ["services/Ordering/Ordering.Domain/Ordering.Domain.csproj", "services/Ordering/Ordering.Domain/"]
COPY ["services/Ordering/Ordering.Persistence.MartenDb/Ordering.Persistence.MartenDb.csproj", "services/Ordering/Ordering.Persistence.MartenDb/"]
COPY ["services/Ordering/Ordering.Persistence.Mongo/Ordering.Persistence.Mongo.csproj", "services/Ordering/Ordering.Persistence.Mongo/"]
COPY ["services/Ordering/Ordering.Application/Ordering.Application.csproj", "services/Ordering/Ordering.Application/"]
COPY ["services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj", "services/Ordering/Ordering.Infrastructure/"]
RUN dotnet restore "services/Ordering/Ordering.IntegrationTests/Ordering.IntegrationTests.csproj"
COPY . .
WORKDIR "/src/services/Ordering/Ordering.IntegrationTests"
RUN dotnet build "Ordering.IntegrationTests.csproj" -c Release -o /app/build

# run the unit tests
FROM build AS test
# set the directory to be within the unit test project

# run the unit tests

RUN dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=OpenCover  Ordering.IntegrationTests.csproj --verbosity normal