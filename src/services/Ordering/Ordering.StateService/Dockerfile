#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["services/Ordering/Ordering.StateService/Ordering.StateService.csproj", "services/Ordering/Ordering.StateService/"]
COPY ["services/Ordering/Ordering.State/Ordering.State.csproj", "services/Ordering/Ordering.State/"]
COPY ["services/Ordering/Ordering.Messages/Ordering.Contracts.csproj", "services/Ordering/Ordering.Messages/"]
COPY ["services/Ordering/Ordering.Domain/Ordering.Domain.csproj", "services/Ordering/Ordering.Domain/"]
COPY ["services/Ordering/Ordering.Domain.Core/Ordering.Domain.Core.csproj", "services/Ordering/Ordering.Domain.Core/"]
RUN dotnet restore "services/Ordering/Ordering.StateService/Ordering.StateService.csproj"
COPY . .
WORKDIR "/src/services/Ordering/Ordering.StateService"
RUN dotnet build "Ordering.StateService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ordering.StateService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.StateService.dll"]