package main

import (
	"gin-redis-api/routes"
	"gin-redis-api/util"
	"log"
)

func main() {

	defer util.RedisClient.Close()

	log.Println("begin gin redis api")

	router := routes.InitRouter()

	router.Run(":9099")
}
