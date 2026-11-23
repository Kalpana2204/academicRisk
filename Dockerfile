# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["AcademicRiskRadar.Web/AcademicRiskRadar.Web.csproj", "AcademicRiskRadar.Web/"]
RUN dotnet restore "AcademicRiskRadar.Web/AcademicRiskRadar.Web.csproj"

COPY . .
WORKDIR "/src/AcademicRiskRadar.Web"
RUN dotnet build "AcademicRiskRadar.Web.csproj" -c Release -o /app/build
RUN dotnet publish "AcademicRiskRadar.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AcademicRiskRadar.Web.dll"]
