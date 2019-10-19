package routes

import (
	"github.com/gin-gonic/gin"

	h "gin-redis-api/handlers"
)

// InitRouter
func InitRouter() *gin.Engine {

	router := gin.Default()

	router.GET("/", h.IndexApi)

	router.GET("/r", h.GetApi)

	router.POST("/r", h.PostApi)

	router.POST("/c", h.AddCountApi)

	return router
}
