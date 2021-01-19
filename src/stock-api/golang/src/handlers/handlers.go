package handlers

import (
	"database/sql"
	"encoding/json"
	"log"
	"net/http" 
	"os"
	"strconv"
	"github.com/gorilla/mux" 
	_ "github.com/lib/pq"
	"stock-api/models"
)

type response struct {
	ID      int64  `json:"id,omitempty"`
	Message string `json:"message,omitempty"`
}

func createConnection() *sql.DB {
	db, err := sql.Open("postgres", os.Getenv("POSTGRES_CONNECTION_STRING"))
	if err != nil {
		panic(err)
	}

	err = db.Ping()
	if err != nil {
		panic(err)
	}

	return db
}

func GetProductStock(w http.ResponseWriter, r *http.Request) {
	params := mux.Vars(r)
	id,_ := strconv.Atoi(params["id"])

	product,_ := getProductStock(int64(id))	
	log.Printf("Fetched stock for product ID: %v", id)

	w.Header().Add("Content-Type", "application/json")
	json.NewEncoder(w).Encode(product)
}

func SetProductStock(w http.ResponseWriter, r *http.Request) {
	params := mux.Vars(r)
	id,_ := strconv.Atoi(params["id"])

	var product models.Product
	_ = json.NewDecoder(r.Body).Decode(&product)

	setProductStock(int64(id), product.Stock)
	log.Printf("Updated stock for product ID: %v", id)

	res := response{
		ID:      int64(id),
		Message: "Stock updated",
	}

	w.Header().Add("Content-Type", "application/json")
	json.NewEncoder(w).Encode(res)
}

func getProductStock(id int64) (models.Product, error) {
	db := createConnection()
	defer db.Close()
	
	sql := `SELECT id, stock FROM "public"."products" WHERE id=$1`
	row := db.QueryRow(sql, id)

	var product models.Product
	err := row.Scan(&product.ID, &product.Stock)

	if err != nil {
		log.Fatalf("Error fetching product. %v", err)
	}

	return product, err
}

func setProductStock(id int64, stock int64) {
	db := createConnection()
	defer db.Close()

	sql := `UPDATE "public"."products" SET stock = $2 WHERE id=$1`
	_, err := db.Exec(sql, id, stock)

	if err != nil {
		log.Fatalf("Error updating product. %v", err)
	}
}
