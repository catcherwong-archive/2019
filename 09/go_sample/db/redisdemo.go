package db

import (
	"fmt"
	"time"

	"github.com/go-redis/redis"
)

type RedisDemo struct {
	Host string
	Pwd  string
	Db   int
}

func (r RedisDemo) getClient() *redis.Client {
	client := redis.NewClient(&redis.Options{
		Addr:     r.Host,
		Password: r.Pwd,
		DB:       r.Db,
	})

	return client
}

func ShowRedisDemo() {
	redisDemo := RedisDemo{Host: "127.0.0.1:6379", Pwd: "", Db: 0}

	redisDemo.PongRedis()

	key := "go:redis:1"

	redisDemo.SetValue(key, "catcher wong", 1000*time.Millisecond)

	redisDemo.GetValue(key)
}

func (r RedisDemo) PongRedis() {
	client := r.getClient()

	pong, err := client.Ping().Result()
	fmt.Println(pong, err)
}

func (r RedisDemo) SetValue(key, val string, t time.Duration) {
	client := r.getClient()

	res, err := client.Set(key, val, t).Result()
	fmt.Println(res, err)
}

func (r RedisDemo) GetValue(key string) {
	client := r.getClient()

	res, err := client.Get(key).Result()

	fmt.Println(res, err)
}
