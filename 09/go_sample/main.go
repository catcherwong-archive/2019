package main

import (
	b "go_sample/basic"
	"go_sample/db"
)

func main() {

	b.ShowBasicDemo()
	b.ShowJsonDemo()
	b.ShowHttpDemo()
	db.ShowRedisDemo()
}
