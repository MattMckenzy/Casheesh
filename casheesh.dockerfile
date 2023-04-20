#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:latest AS base
WORKDIR /app
EXPOSE 80

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/nightly/sdk:8.0-preview AS publish
ARG TARGETARCH
ARG CONFIG
WORKDIR /src
COPY ["Casheesh.csproj", "."]
RUN dotnet restore "./Casheesh.csproj" -a $TARGETARCH
COPY . .
WORKDIR "/src/."
RUN dotnet publish "Casheesh.csproj" -c $CONFIG -o /app/publish -a $TARGETARCH

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Casheesh.dll"]