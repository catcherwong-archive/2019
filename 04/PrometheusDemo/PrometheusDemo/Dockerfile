FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY PrometheusDemo/PrometheusDemo.csproj PrometheusDemo/
RUN dotnet restore PrometheusDemo/PrometheusDemo.csproj
COPY . .
WORKDIR /src/PrometheusDemo
RUN dotnet build PrometheusDemo.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish PrometheusDemo.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PrometheusDemo.dll"]
