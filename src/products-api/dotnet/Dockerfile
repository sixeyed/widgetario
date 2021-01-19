FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS builder

WORKDIR /src
COPY Widgetario.ProductApi/Widgetario.ProductApi.csproj .
RUN dotnet restore

COPY Widgetario.ProductApi/ .
RUN dotnet publish -c Release -o /out Widgetario.ProductApi.csproj

# app image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine

ENTRYPOINT ["dotnet", "/app/Widgetario.ProductApi.dll"]
ENV ConnectionStrings__ProductsDb="server=products-db;database=postgres;user id=postgres;password=widgetario"

WORKDIR /app
COPY --from=builder /out/ .