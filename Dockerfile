# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY EthereumTransactions.sln .
COPY EthereumTransactions/EthereumTransactions.csproj EthereumTransactions/EthereumTransactions.csproj
COPY EthereumTransactions.UnitTests/EthereumTransactions.UnitTests.csproj EthereumTransactions.UnitTests/EthereumTransactions.UnitTests.csproj
COPY EthereumTransactions.IntegrationTests/EthereumTransactions.IntegrationTests.csproj EthereumTransactions.IntegrationTests/EthereumTransactions.IntegrationTests.csproj
RUN dotnet restore

COPY . .
RUN dotnet build "EthereumTransactions/EthereumTransactions.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EthereumTransactions/EthereumTransactions.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EthereumTransactions.dll"]
