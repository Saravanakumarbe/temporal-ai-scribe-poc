FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/TemporalAiScribe.Api/TemporalAiScribe.Api.csproj", "src/TemporalAiScribe.Api/"]
RUN dotnet restore "src/TemporalAiScribe.Api/TemporalAiScribe.Api.csproj"
COPY . .
WORKDIR "/src/src/TemporalAiScribe.Api"
RUN dotnet publish "TemporalAiScribe.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TemporalAiScribe.Api.dll"]
