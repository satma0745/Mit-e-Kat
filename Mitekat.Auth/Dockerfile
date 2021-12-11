FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Mitekat.Auth.Core/Mitekat.Auth.Core.csproj", "./Mitekat.Auth.Core/"]
COPY ["Mitekat.Auth.Domain/Mitekat.Auth.Domain.csproj", "./Mitekat.Auth.Domain/"]
COPY ["Mitekat.Auth.Helpers/Mitekat.Auth.Helpers.csproj", "./Mitekat.Auth.Helpers/"]
COPY ["Mitekat.Auth.Persistence/Mitekat.Auth.Persistence.csproj", "./Mitekat.Auth.Persistence/"]
COPY ["Mitekat.Auth.Web/Mitekat.Auth.Web.csproj", "./Mitekat.Auth.Web/"]
COPY ["Mitekat.Auth.sln", "./"]
RUN dotnet restore "Mitekat.Auth.sln"
COPY . .
WORKDIR /src
RUN dotnet build "Mitekat.Auth.sln" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mitekat.Auth.sln" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mitekat.Auth.Web.dll"]