FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["ExchangeAnalyticsService/ExchangeAnalyticsService.csproj", "ExchangeAnalyticsService/"]
COPY ["../../CommonProjects/CommonProjects/MySqlWrapper/MySqlWrapper.csproj", "../../CommonProjects/CommonProjects/MySqlWrapper/"]
COPY ["../../CommonProjects/CommonProjects/DbWrapperCore/DbWrapperCore.csproj", "../../CommonProjects/CommonProjects/DbWrapperCore/"]
COPY ["ExchCommonLib/ExchCommonLib.csproj", "ExchCommonLib/"]
COPY ["../../CommonProjects/CommonProjects/TechAnalysisAlgLib/TechAnalysisAlgLib.csproj", "../../CommonProjects/CommonProjects/TechAnalysisAlgLib/"]
COPY ["../../CommonProjects/CommonProjects/AuthCommonLib/AuthCommonLib.csproj", "../../CommonProjects/CommonProjects/AuthCommonLib/"]
RUN dotnet restore "ExchangeAnalyticsService/ExchangeAnalyticsService.csproj"
COPY . .
WORKDIR "/src/ExchangeAnalyticsService"
RUN dotnet build "ExchangeAnalyticsService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExchangeAnalyticsService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeAnalyticsService.dll"]