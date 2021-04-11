FROM golang:1.15.6 AS builder
ENV CGO_ENABLED=0

WORKDIR /go/stock-api
COPY ./src/go.mod .
RUN go mod download 

COPY ./src .
RUN go build -o /server

# app
FROM alpine:3.13

ENV POSTGRES_CONNECTION_STRING="host=products-db port=5432 user=postgres password=widgetario dbname=postgres sslmode=disable"
EXPOSE 8080
CMD ["/app/server"]

WORKDIR /cache
WORKDIR /app
COPY --from=builder /server .