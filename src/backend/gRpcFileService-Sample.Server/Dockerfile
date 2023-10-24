#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["gRpcFileService-Sample.Server/gRpcFileService-Sample.Server.csproj", "gRpcFileService-Sample.Server/"]
RUN dotnet restore "gRpcFileService-Sample.Server/gRpcFileService-Sample.Server.csproj"
COPY . .
WORKDIR "/src/gRpcFileService-Sample.Server"
RUN dotnet build "gRpcFileService-Sample.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "gRpcFileService-Sample.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "gRpcFileService-Sample.Server.dll"]