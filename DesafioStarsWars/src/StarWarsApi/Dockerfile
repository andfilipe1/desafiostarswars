FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY src/StarWarsApi/StarWarsApi.csproj src/StarWarsApi/
COPY src/StarWars.Model/StarWars.Model.csproj src/StarWars.Model/
COPY src/StarWars.Helpers/StarWars.Helpers.csproj src/StarWars.Helpers/
COPY src/StarWars.Infra/StarWars.Infra.csproj src/StarWars.Infra/
RUN dotnet restore src/StarWarsApi/StarWarsApi.csproj
COPY . .
WORKDIR /src/src/StarWarsApi
RUN dotnet build StarWarsApi.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish StarWarsApi.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "StarWarsApi.dll"]
