#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS build
RUN apt-get update && apt-get install -y libc6-dev && apt-get install -y libgdiplus

WORKDIR /src
COPY ["CST.Api/CST.Api.csproj", "CST.Api/"]
COPY ["CST.BusinessLogic/CST.BusinessLogic.csproj", "CST.BusinessLogic/"]
COPY ["CST.Common/CST.Common.csproj", "CST.Common/"]
COPY ["CST.Dal/CST.Dal.csproj", "CST.Dal/"]
RUN dotnet restore "CST.Api/CST.Api.csproj"

COPY . .
WORKDIR "/src/CST.Api"
RUN dotnet build "CST.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CST.Api.csproj" -c Release -o /app/publish

FROM base AS final
RUN apt-get update && apt-get install -y libc6-dev && apt-get install -y libgdiplus
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT Docker
ENV BUILD_VERSION ${BUILD_VERSION}
ENTRYPOINT ["dotnet", "CST.Api.dll"]