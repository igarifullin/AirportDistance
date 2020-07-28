#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["sources/presentation/cTeleport.AirportMeasure.Api/cTeleport.AirportMeasure.Api.csproj", "sources/presentation/cTeleport.AirportMeasure.Api/"]
COPY ["sources/infrastructure/cTeleport.AirportMeasure.DataAccess/cTeleport.AirportMeasure.DataAccess.csproj", "sources/infrastructure/cTeleport.AirportMeasure.DataAccess/"]
COPY ["sources/core/cTeleport.AirportMeasure.Services/cTeleport.AirportMeasure.Services.csproj", "sources/core/cTeleport.AirportMeasure.Services/"]
COPY ["sources/core/cTeleport.AirportMeasure.Core/cTeleport.AirportMeasure.Core.csproj", "sources/core/cTeleport.AirportMeasure.Core/"]
COPY ["sources/utils/cTeleport.AirportMeasure.Utils/cTeleport.AirportMeasure.Utils.csproj", "sources/utils/cTeleport.AirportMeasure.Utils/"]
COPY ["sources/core/cTeleport.AirportMeasure.Data/cTeleport.AirportMeasure.Data.csproj", "sources/core/cTeleport.AirportMeasure.Data/"]
RUN dotnet restore "sources/presentation/cTeleport.AirportMeasure.Api/cTeleport.AirportMeasure.Api.csproj"
COPY . .
WORKDIR "/src/sources/presentation/cTeleport.AirportMeasure.Api"
RUN dotnet build "cTeleport.AirportMeasure.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "cTeleport.AirportMeasure.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "cTeleport.AirportMeasure.Api.dll"]