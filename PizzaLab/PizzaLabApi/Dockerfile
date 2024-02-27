FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PizzaLabApi/PizzaLabApi.csproj", "PizzaLabApi/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["BusinessObject2/BusinessObject.csproj", "BusinessObject2/"]
RUN dotnet restore "PizzaLabApi/PizzaLabApi.csproj"
COPY . .
WORKDIR "/src/PizzaLabApi"
RUN dotnet build "PizzaLabApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PizzaLabApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PizzaLabApi.dll"]
