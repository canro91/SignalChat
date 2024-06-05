FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SignalChat/SignalChat.csproj", "SignalChat/"]
COPY ["SignalChat.Bot/SignalChat.Bot.csproj", "SignalChat.Bot/"]
COPY ["SignalChat.Core/SignalChat.Core.csproj", "SignalChat.Core/"]
COPY ["SignalChat.Database/SignalChat.Database.csproj", "SignalChat.Database/"]
RUN dotnet restore "./SignalChat/SignalChat.csproj"
COPY . .
WORKDIR "/src/SignalChat"
RUN dotnet build "./SignalChat.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SignalChat.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SignalChat.dll"]