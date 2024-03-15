FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["LM1_myjnia/LM1_myjnia.csproj", "LM1_myjnia/"]
RUN dotnet restore "LM1_myjnia/LM1_myjnia.csproj"
COPY . .
WORKDIR "/src/LM1_myjnia"
RUN dotnet build "LM1_myjnia.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LM1_myjnia.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LM1_myjnia.dll"]
