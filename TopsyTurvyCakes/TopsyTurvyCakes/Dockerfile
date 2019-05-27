FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["TopsyTurvyCakes/TopsyTurvyCakes.csproj", "TopsyTurvyCakes/"]
RUN dotnet restore "TopsyTurvyCakes/TopsyTurvyCakes.csproj"
COPY . .
WORKDIR "/src/TopsyTurvyCakes"
RUN dotnet build "TopsyTurvyCakes.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TopsyTurvyCakes.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TopsyTurvyCakes.dll"]