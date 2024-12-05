# Phase de construction
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BACK_Nuit2Info/BACK_Nuit2Info.csproj", "./"]
RUN dotnet restore "./BACK_Nuit2Info.csproj"
COPY . .
WORKDIR "/src/BACK_Nuit2Info"
RUN dotnet build "BACK_Nuit2Info.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Phase de publication
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BACK_Nuit2Info.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Phase finale (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BACK_Nuit2Info.dll"]