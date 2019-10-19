package main

import (
	"gin-redis-api/routes"
	"log"
)

func main() {

	log.Println("begin gin redis api")

	router := routes.InitRouter()

	router.Run(":9099")
}
