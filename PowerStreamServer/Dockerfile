#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["PowerStreamServer/PowerStreamServer.csproj", "PowerStreamServer/"]
COPY ["Harmonic/Harmonic.csproj", "Harmonic/"]
RUN dotnet restore "PowerStreamServer/PowerStreamServer.csproj"
COPY . .
WORKDIR "/src/PowerStreamServer"
RUN dotnet build "PowerStreamServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PowerStreamServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8189
ENTRYPOINT ["dotnet", "PowerStreamServer.dll"]

