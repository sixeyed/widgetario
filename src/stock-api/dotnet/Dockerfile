FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS builder

WORKDIR /src
COPY /Widgetario.StockApi/Widgetario.StockApi.csproj .
RUN dotnet restore

COPY Widgetario.StockApi/ .
RUN dotnet publish -c Release -o /out Widgetario.StockApi.csproj

# app image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine

ENTRYPOINT ["dotnet", "/app/Widgetario.StockApi.dll"]
ENV ConnectionStrings__StockDb="server=products-db;database=postgres;user id=postgres;password=widgetario"

WORKDIR /app
COPY --from=builder /out/ .