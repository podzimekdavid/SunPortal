FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SunPortal.Cloud.Services.Portal/SunPortal.Cloud.Services.Portal.csproj", "SunPortal.Cloud.Services.Portal/"]
RUN dotnet restore "SunPortal.Cloud.Services.Portal/SunPortal.Cloud.Services.Portal.csproj"
COPY . .
WORKDIR "/src/SunPortal.Cloud.Services.Portal"
RUN dotnet build "SunPortal.Cloud.Services.Portal.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SunPortal.Cloud.Services.Portal.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV TZ Europe/Prague

ENTRYPOINT ["dotnet", "SunPortal.Cloud.Services.Portal.dll"]
