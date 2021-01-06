#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["web/Burgers.WebSPA/Burgers.WebSPA.csproj", "web/Burgers.WebSPA/"]
RUN dotnet restore "web/Burgers.WebSPA/Burgers.WebSPA.csproj"
COPY . .
WORKDIR "/src/web/Burgers.WebSPA"
RUN dotnet build "Burgers.WebSPA.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Burgers.WebSPA.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Burgers.WebSPA.dll"]
