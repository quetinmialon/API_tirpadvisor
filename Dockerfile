# Étape 1 : Image de base (runtime only)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8000
EXPOSE 8001

# Étape 2 : Image de build (SDK pour compiler l'application)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copier uniquement les fichiers projet et restaurer les dépendances (optimisation du cache Docker)
COPY ["tripAdvisorAPI.csproj", "./"]
RUN dotnet restore

# Copier le reste des fichiers
COPY . .

# Compiler l'application (au lieu de publier)
RUN dotnet build -c Release -o /app/build

# Étape 3 : Image finale pour exécuter l'application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copier les fichiers compilés depuis l'étape précédente
COPY --from=build /app/build .

# Vérification et lien du fichier de secret avant de lancer l'application
CMD ["/bin/sh", "-c", "[ ! -e /app/firebase-key.json ] && ln -s /etc/secrets/firebase-key.json /app/firebase-key.json; dotnet tripAdvisorAPI.dll"]

# Définir l'environnement en production
ENV ASPNETCORE_ENVIRONMENT=Production
