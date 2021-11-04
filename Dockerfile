FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["PaginacaoProject.csproj", "./"]
RUN dotnet restore "PaginacaoProject.csproj"
COPY . .
WORKDIR "/src/PaginacaoProject"
RUN dotnet build "PaginacaoProject.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PaginacaoProject.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaginacaoProject.dll"]
