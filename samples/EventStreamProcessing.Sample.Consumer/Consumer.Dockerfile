FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["EventStreamProcessing.Sample.Consumer.csproj", "./"]
RUN dotnet restore "EventStreamProcessing.Sample.Consumer.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "EventStreamProcessing.Sample.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventStreamProcessing.Sample.Consumer.csproj" -c Release -o /app/publish

FROM base AS final

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventStreamProcessing.Sample.Consumer.dll"]

# dotnet run

# docker build -t event-stream-sample-consumer --file Consumer.Dockerfile .

# docker compose build
# docker-compose up 