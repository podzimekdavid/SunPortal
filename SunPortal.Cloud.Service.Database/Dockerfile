FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SunPortal.Cloud.Service.Database/SunPortal.Cloud.Service.Database.csproj", "SunPortal.Cloud.Service.Database/"]
RUN dotnet restore "SunPortal.Cloud.Service.Database/SunPortal.Cloud.Service.Database.csproj"
COPY . .
WORKDIR "/src/SunPortal.Cloud.Service.Database"
RUN dotnet build "SunPortal.Cloud.Service.Database.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SunPortal.Cloud.Service.Database.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SunPortal.Cloud.Service.Database.dll"]
