FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["src/Stashy.Api/Stashy.Api.csproj", "src/Stashy.Api/"]
RUN dotnet restore "src/Stashy.Api/Stashy.Api.csproj"
COPY . .
WORKDIR "/src/src/Stashy.Api"
RUN dotnet build "Stashy.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Stashy.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Stashy.Api.dll"]