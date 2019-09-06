package main

import (
	b "go_sample/basic"
	_ "go_sample/db"
	"go_sample/gormdemo"
)

func main() {

	b.ShowBasicDemo()
	// b.ShowJsonDemo()
	// b.ShowHttpDemo()
	// db.ShowPgDemo()
	// db.ShowRedisDemo()

	gormdemo.Show()
}
