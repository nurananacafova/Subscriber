FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Task8.csproj", "."]
RUN dotnet restore "Task8.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Task8.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Task8.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Task8.dll"]
