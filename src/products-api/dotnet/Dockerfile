FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS builder

WORKDIR /src
COPY Widgetario.ProductApi/Widgetario.ProductApi.csproj .
RUN dotnet restore

COPY Widgetario.ProductApi/ .
RUN dotnet publish -c Release -o /out Widgetario.ProductApi.csproj

# app image
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine

ENV ASPNETCORE_HTTP_PORTS=80 \
    ConnectionStrings__ProductsDb="server=products-db;database=postgres;user id=postgres;password=widgetario"

ENTRYPOINT ["dotnet", "/app/Widgetario.ProductApi.dll"]

WORKDIR /app
COPY --from=builder /out/ .
