FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ESCenter.Api/ESCenter.Api.csproj", "ESCenter.Api/"]
COPY ["ESCenter.Infrastructure/ESCenter.Infrastructure.csproj", "ESCenter.Infrastructure/"]
COPY ["ESCenter.Application/ESCenter.Application.csproj", "ESCenter.Application/"]
COPY ["ESCenter.Domain.Shared/ESCenter.Domain.Shared.csproj", "ESCenter.Domain.Shared/"]
COPY ["Matt.SharedKernel/Matt.SharedKernel.csproj", "Matt.SharedKernel/"]
COPY ["Matt.Auditing/Matt.Auditing.csproj", "Matt.Auditing/"]
COPY ["Matt.AutoDI/Matt.AutoDI.csproj", "Matt.AutoDI/"]
COPY ["Matt.Paginated/Matt.Paginated.csproj", "Matt.Paginated/"]
COPY ["Matt.ResultObject/Matt.ResultObject.csproj", "Matt.ResultObject/"]
COPY ["ESCenter.Domain/ESCenter.Domain.csproj", "ESCenter.Domain/"]
COPY ["ESCenter.Persistence/ESCenter.Persistence.csproj", "ESCenter.Persistence/"]
COPY ["ESCenter.Host/ESCenter.Host.csproj", "ESCenter.Host/"]
COPY ["ESCenter.Mobile.Application/ESCenter.Mobile.Application.csproj", "ESCenter.Mobile.Application/"]
RUN dotnet restore "ESCenter.Api/ESCenter.Api.csproj"
COPY . .
WORKDIR "/src/ESCenter.Api"
RUN dotnet build "ESCenter.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ESCenter.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ESCenter.Api.dll"]
