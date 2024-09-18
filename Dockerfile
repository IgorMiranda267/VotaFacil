# Estágio 1: Compilação dos projetos
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG DATABASE_URL
ENV DATABASE_URL=${DATABASE_URL}
WORKDIR /src
COPY src .

WORKDIR /src/VotaFacil.WebUI
RUN dotnet publish VotaFacil.WebUI.csproj -c Release -o /app/publish

# Estágio 2: Construção da imagem final
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Expondo as portas necessárias
EXPOSE 80

# Definindo o ponto de entrada
ENTRYPOINT ["dotnet", "VotaFacil.WebUI.dll"]