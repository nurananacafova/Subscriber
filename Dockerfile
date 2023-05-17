#FROM nurananajafova/subscriberservice:v2
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app   

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Task8/SubscriberService.csproj", "Task8/"]
RUN dotnet restore "Task8/SubscriberService.csproj" 
COPY . .
WORKDIR "/app/Task8"
RUN dotnet build "SubscriberService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SubscriberService.csproj" -c Release -o /app/publish
 

FROM base AS finale
WORKDIR /app 
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SubscriberService.dll"]