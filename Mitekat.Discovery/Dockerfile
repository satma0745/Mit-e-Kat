FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Mitekat.Discovery.Application/Mitekat.Discovery.Application.csproj", "./Mitekat.Discovery.Application/"]
COPY ["Mitekat.Discovery.Domain/Mitekat.Discovery.Domain.csproj", "./Mitekat.Discovery.Domain/"]
COPY ["Mitekat.Discovery.sln", "./"]
RUN dotnet restore "Mitekat.Discovery.sln"
COPY . .
WORKDIR /src
RUN dotnet build "Mitekat.Discovery.sln" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mitekat.Discovery.sln" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mitekat.Discovery.Application.dll"]