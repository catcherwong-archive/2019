FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY BService/BService.csproj BService/
RUN dotnet restore BService/BService.csproj
COPY . .
WORKDIR /src/BService
RUN dotnet build BService.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish BService.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BService.dll"]
