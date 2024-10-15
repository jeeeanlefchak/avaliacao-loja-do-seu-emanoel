FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 2225

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EmpacotamentoAPI/EmpacotamentoAPI.csproj", "EmpacotamentoAPI/"]
RUN dotnet restore "EmpacotamentoAPI/EmpacotamentoAPI.csproj"
COPY . .
WORKDIR "/src/EmpacotamentoAPI"
RUN dotnet build "EmpacotamentoAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmpacotamentoAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmpacotamentoAPI.dll"]