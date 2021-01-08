#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["PowerStreamServer/PowerStream.Server.csproj", "PowerStreamServer/"]
COPY ["Harmonic/Harmonic.csproj", "Harmonic/"]
COPY ["PowerStream.Core/PowerStream.Core.csproj", "PowerStream.Core/"]
COPY ["Xabe.FFmpeg/Xabe.FFmpeg.csproj", "Xabe.FFmpeg/"]
COPY ["Power.WebStream/Power.WebStream.csproj", "Power.WebStream/"]
RUN dotnet restore "PowerStreamServer/PowerStream.Server.csproj"
COPY . .
WORKDIR "/src/PowerStreamServer"
RUN dotnet build "PowerStream.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PowerStream.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PowerStream.Server.dll"]