#FROM nurananajafova/subscriberservice:v2
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS c
WORKDIR /app   

    
COPY ["Task8/SubscriberService.csproj", "Task8/"]
#COPY ["reference1/reference1.csproj", "reference1/"]
#others reference if necesary



RUN dotnet restore "Task8/SubscriberService.csproj" 

COPY . .
WORKDIR "/app/Task8"
RUN dotnet build "SubscriberService.csproj" -c Release -o /app/build

from c as publish
RUN dotnet publish "SubscriberService.csproj" -c Release -o /app/publish
 

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app 
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SubscriberService.dll"]