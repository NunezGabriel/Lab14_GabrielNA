FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Lab13_GabrielNA/Lab13_GabrielNA.csproj Lab13_GabrielNA/
COPY Lab13_GabrielNA.Application/Lab13_GabrielNA.Application.csproj Lab13_GabrielNA.Application/
COPY Lab13_GabrielNA.Domain/Lab13_GabrielNA.Domain.csproj Lab13_GabrielNA.Domain/
COPY Lab13_GabrielNA.Infrastructure/Lab13_GabrielNA.Infrastructure.csproj Lab13_GabrielNA.Infrastructure/
RUN dotnet restore Lab13_GabrielNA/Lab13_GabrielNA.csproj

COPY . .
RUN dotnet publish Lab13_GabrielNA/Lab13_GabrielNA.csproj --configuration Release --no-restore --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
RUN apt-get update \
    && apt-get install --yes --no-install-recommends libgssapi-krb5-2 \
    && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
ENV TZ=America/Lima
EXPOSE 8080

ENTRYPOINT ["dotnet", "Lab13_GabrielNA.dll"]
