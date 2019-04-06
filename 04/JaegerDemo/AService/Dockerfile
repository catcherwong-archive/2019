FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY AService/AService.csproj AService/
RUN dotnet restore AService/AService.csproj
COPY . .
WORKDIR /src/AService
RUN dotnet build AService.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish AService.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AService.dll"]
