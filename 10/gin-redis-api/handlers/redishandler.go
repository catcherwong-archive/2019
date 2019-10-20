package handlers

import (
	"gin-redis-api/util"
	"github.com/gin-gonic/gin"
	"log"
	"net/http"
	"time"
)

const KEY = "gin:api:name"

func GetApi(c *gin.Context) {

	r, err := util.RedisClient.Get(KEY).Result()

	log.Println(r, err)

	if err != nil {
		log.Println("can not get value from redis")
	}

	buildGinJson(c, 0, "ok", r)
}

func PostApi(c *gin.Context) {

	name := c.Query("name")

	msg := "ok"

	if name == "" {
		msg = "name can not be empty"
		buildGinJson(c, 1000, msg, nil)
		return
	}

	err := util.RedisClient.Set(KEY, name, 30*time.Second).Err()

	if err != nil {
		msg = "set the name to redis occur error"
		buildGinJson(c, 1000, msg, nil)
		return
	}

	buildGinJson(c, 0, msg, nil)
}

func IndexApi(c *gin.Context) {

	c.String(200, "This is index page.")

}

func AddCountApi(c *gin.Context) {

	type CountName struct {
		Name string `json:"name"`
	}

	var cn CountName

	err := c.BindJSON(&cn)

	if err != nil || cn.Name == "" {
		log.Println("error here")
		buildGinJson(c, 1000, "param error", nil)
		return
	}

	v, err := util.RedisClient.Incr(cn.Name).Result()

	if err != nil {
		log.Println("incr error")
		buildGinJson(c, 1001, "redis incr error", nil)
		return
	}

	buildGinJson(c, 0, "ok", v)
}

func buildGinJson(c *gin.Context, code int, msg string, data interface{}) {
	c.JSON(http.StatusOK, gin.H{
		"code": code,
		"msg":  msg,
		"data": data,
	})
}
