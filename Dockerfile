#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["samples/EventStreamProcessing.Sample.Worker/EventStreamProcessing.Sample.Worker.csproj", "samples/EventStreamProcessing.Sample.Worker/"]
COPY ["src/EventStreamProcessing.Abstractions/EventStreamProcessing.Abstractions.csproj", "src/EventStreamProcessing.Abstractions/"]
COPY ["src/EventStreamProcessing.Kafka/EventStreamProcessing.Kafka.csproj", "src/EventStreamProcessing.Kafka/"]
RUN dotnet restore "samples/EventStreamProcessing.Sample.Worker/EventStreamProcessing.Sample.Worker.csproj"
COPY . .
WORKDIR "/src/samples/EventStreamProcessing.Sample.Worker"
RUN dotnet build "EventStreamProcessing.Sample.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventStreamProcessing.Sample.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventStreamProcessing.Sample.Worker.dll"]

# docker build -t event-stream-worker .