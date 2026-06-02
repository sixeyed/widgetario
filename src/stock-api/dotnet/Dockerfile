FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS builder

WORKDIR /src
COPY /Widgetario.StockApi/Widgetario.StockApi.csproj .
RUN dotnet restore

COPY Widgetario.StockApi/ .
RUN dotnet publish -c Release -o /out Widgetario.StockApi.csproj

# app image
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine

ENV ASPNETCORE_HTTP_PORTS=80 \
    ConnectionStrings__StockDb="server=products-db;database=postgres;user id=postgres;password=widgetario"

ENTRYPOINT ["dotnet", "/app/Widgetario.StockApi.dll"]

WORKDIR /app
COPY --from=builder /out/ .
