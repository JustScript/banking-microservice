FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5171

ENV ASPNETCORE_URLS=http://+:5171

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["BankingMicroservice/BankingMicroservice.csproj", "BankingMicroservice/"]
RUN dotnet restore "BankingMicroservice/BankingMicroservice.csproj"
COPY . .
WORKDIR "/src/BankingMicroservice"
RUN dotnet build "BankingMicroservice.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "BankingMicroservice.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankingMicroservice.dll"]
