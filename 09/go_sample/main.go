package main

import (
	b "go_sample/basic"
	"go_sample/config"
	_ "go_sample/db"
	_ "go_sample/gormdemo"
)

func main() {

	b.ShowBasicDemo()
	// b.ShowJsonDemo()
	// b.ShowHttpDemo()
	// db.ShowPgDemo()
	// db.ShowRedisDemo()

	// flag.Parse()

	// gormdemo.Show()
	// config.ShowIni()
	config.ShowYaml()
}
