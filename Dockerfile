FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["GymTracker.slnx", "./"]
COPY ["GymTracker.Application/GymTracker.Application.csproj", "GymTracker.Application/"]
COPY ["GymTracker.Domain/GymTracker.Domain.csproj", "GymTracker.Domain/"]
COPY ["GymTracker.Infrastructure/GymTracker.Infrastructure.csproj", "GymTracker.Infrastructure/"]
COPY ["GymTracker.WebApi/GymTracker.WebApi.csproj", "GymTracker.WebApi/"]

RUN dotnet restore "GymTracker.WebApi/GymTracker.WebApi.csproj"

COPY . .
WORKDIR "/src/GymTracker.WebApi"
RUN dotnet build "GymTracker.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR "/src/GymTracker.WebApi"
RUN dotnet publish "GymTracker.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GymTracker.WebApi.dll"]