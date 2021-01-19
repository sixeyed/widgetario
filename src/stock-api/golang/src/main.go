package main

import (
	"log"
	"net/http"
	"stock-api/router"
	"github.com/prometheus/client_golang/prometheus"
	"github.com/prometheus/client_golang/prometheus/promauto"	
)

var (
	appInfo = promauto.NewGaugeVec(prometheus.GaugeOpts{
	  Name: "app_info",
	  Help: "Application info",
	}, []string{"version", "goversion"})
)

func main() {
	appInfo.WithLabelValues("0.3.0", "1.14.4").Set(1)
	
	r := router.Router()
	log.Println("Starting server on the port 8080...")
	log.Fatal(http.ListenAndServe(":8080", r))
}
