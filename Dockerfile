# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files first (layer caching — only re-runs if these change)
COPY *.slnx ./
COPY TradingJournal.Api/TradingJournal.Api.csproj TradingJournal.Api/
COPY TradingJournal.Application/TradingJournal.Application.csproj TradingJournal.Application/
COPY TradingJournal.Domain/TradingJournal.Domain.csproj TradingJournal.Domain/
COPY TradingJournal.Infrastructure/TradingJournal.Infrastructure.csproj TradingJournal.Infrastructure/
COPY TradingJournal.Contract/TradingJournal.Contract.csproj TradingJournal.Contract/

# Restore dependencies
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish TradingJournal.Api/TradingJournal.Api.csproj -c Release -o /app/publish

# Runtime stage (smaller image — no SDK, just the runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Create uploads folder
RUN mkdir -p uploads

EXPOSE 8080
ENTRYPOINT ["dotnet", "TradingJournal.Api.dll"]
