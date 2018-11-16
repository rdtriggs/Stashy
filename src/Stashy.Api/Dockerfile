FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 50797
EXPOSE 44317

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY *.csproj ./
RUN dotnet restore
COPY . .
RUN dotnet build "Stashy.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Stashy.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Stashy.Api.dll"]