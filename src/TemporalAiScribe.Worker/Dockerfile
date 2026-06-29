FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/TemporalAiScribe.Worker/TemporalAiScribe.Worker.csproj", "src/TemporalAiScribe.Worker/"]
RUN dotnet restore "src/TemporalAiScribe.Worker/TemporalAiScribe.Worker.csproj"
COPY . .
WORKDIR "/src/src/TemporalAiScribe.Worker"
RUN dotnet publish "TemporalAiScribe.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TemporalAiScribe.Worker.dll"]
