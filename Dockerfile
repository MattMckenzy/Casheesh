#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0.13-focal-arm64v8 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0.404-focal-arm64v8 AS build
WORKDIR /src
COPY ["Casheesh.csproj", "."]
RUN dotnet restore "./Casheesh.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Casheesh.csproj" -c Release -o /app/build -r linux-arm

FROM build AS publish
RUN dotnet publish "Casheesh.csproj" -c Release -o /app/publish -r linux-arm

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Casheesh.dll"]