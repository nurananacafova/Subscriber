FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SubscriberService.csproj", "."]
RUN dotnet restore "SubscriberService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SubscriberService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SubscriberService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SubscriberService.dll"]
