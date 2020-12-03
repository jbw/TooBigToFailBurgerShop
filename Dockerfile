#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["TooBigToFailBurgerShop.csproj", ""]
RUN dotnet restore "./TooBigToFailBurgerShop.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TooBigToFailBurgerShop.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TooBigToFailBurgerShop.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TooBigToFailBurgerShop.dll"]