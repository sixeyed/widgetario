FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS builder

WORKDIR /src
COPY Widgetario.Web/Widgetario.Web.csproj .
RUN dotnet restore

COPY Widgetario.Web/ .
RUN dotnet publish -c Release -o /out Widgetario.Web.csproj --no-restore

# app image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

ENV Widgetario__ProductsApi__Url="http://products-api/products" \
    Widgetario__StockApi__Url="http://stock-api:8080/stock" \
    Widgetario__Theme="light"

ENTRYPOINT ["dotnet", "/app/Widgetario.Web.dll"]

# create user & writeable paths
RUN addgroup -S -g 1000 app && \
    adduser -u 1000 -G app -s /bin/false -H -S app && \
    mkdir -p /logs && \
    chown app:app /logs

WORKDIR /app
COPY --from=builder /out/ .