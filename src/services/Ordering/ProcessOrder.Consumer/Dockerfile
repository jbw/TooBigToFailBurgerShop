#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["services/Ordering/ProcessOrder.Consumer/ProcessOrder.Consumer.csproj", "services/Ordering/ProcessOrder.Consumer/"]
COPY ["services/Ordering/Ordering.Messages/Ordering.Contracts.csproj", "services/Ordering/Ordering.Messages/"]
RUN dotnet restore "services/Ordering/ProcessOrder.Consumer/ProcessOrder.Consumer.csproj"
COPY . .
WORKDIR "/src/services/Ordering/ProcessOrder.Consumer"
RUN dotnet build "ProcessOrder.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProcessOrder.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProcessOrder.Consumer.dll"]