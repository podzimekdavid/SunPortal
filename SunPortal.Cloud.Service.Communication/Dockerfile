FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SunPortal.Cloud.Service.Communication/SunPortal.Cloud.Service.Communication.csproj", "SunPortal.Cloud.Service.Communication/"]
RUN dotnet restore "SunPortal.Cloud.Service.Communication/SunPortal.Cloud.Service.Communication.csproj"
COPY . .
WORKDIR "/src/SunPortal.Cloud.Service.Communication"
RUN dotnet build "SunPortal.Cloud.Service.Communication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SunPortal.Cloud.Service.Communication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV TZ Europe/Prague

ENTRYPOINT ["dotnet", "SunPortal.Cloud.Service.Communication.dll"]
