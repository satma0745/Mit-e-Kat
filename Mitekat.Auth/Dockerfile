FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Mitekat.Auth.Application/Mitekat.Auth.Application.csproj", "./Mitekat.Auth.Application/"]
COPY ["Mitekat.Auth.Domain/Mitekat.Auth.Domain.csproj", "./Mitekat.Auth.Domain/"]
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
ENTRYPOINT ["dotnet", "Mitekat.Auth.Application.dll"]