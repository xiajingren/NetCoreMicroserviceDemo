#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["IDS4.AuthCenter/IDS4.AuthCenter.csproj", "IDS4.AuthCenter/"]
RUN dotnet restore "IDS4.AuthCenter/IDS4.AuthCenter.csproj"
COPY . .
WORKDIR "/src/IDS4.AuthCenter"
RUN dotnet build "IDS4.AuthCenter.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IDS4.AuthCenter.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IDS4.AuthCenter.dll"]