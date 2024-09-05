# Estágio 1: Compilação dos projetos
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src .

WORKDIR /src/VotaFacil.WebUI
RUN dotnet publish VotaFacil.WebUI.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY --from=build /app/publish .

# ENV ASPNETCORE_URLS = http://*:8080
# ENV ASPNETCORE_HTTP_PORTS = 8080
EXPOSE 8080
EXPOSE 443
ENTRYPOINT ["dotnet", "VotaFacil.WebUI.dll"]