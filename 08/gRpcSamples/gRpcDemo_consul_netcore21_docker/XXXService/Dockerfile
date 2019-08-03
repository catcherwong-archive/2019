FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY XXXService/XXXService.csproj XXXService/
RUN dotnet restore XXXService/XXXService.csproj
COPY . .
WORKDIR /src/XXXService
RUN dotnet build XXXService.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish XXXService.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "XXXService.dll"]
