FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SunPortal.Cloud.Proxy/SunPortal.Cloud.Proxy.csproj", "SunPortal.Cloud.Proxy/"]
RUN dotnet restore "SunPortal.Cloud.Proxy/SunPortal.Cloud.Proxy.csproj"
COPY . .
WORKDIR "/src/SunPortal.Cloud.Proxy"
RUN dotnet build "SunPortal.Cloud.Proxy.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SunPortal.Cloud.Proxy.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SunPortal.Cloud.Proxy.dll"]
