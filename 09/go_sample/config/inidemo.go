package config

import (
	"fmt"
	"os"

	"flag"

	"github.com/go-ini/ini"
)

func ShowIni() {

	flag.Parse()

	fmt.Println(*env)

	f := fmt.Sprintf("config/demo.%s.ini", *env)

	cfg, err := ini.Load(f)

	if err != nil {
		fmt.Printf("Fail to read file: %v", err)
		os.Exit(1)
	}

	fmt.Println("App Mode:", cfg.Section("").Key("app_mode").String())

	fmt.Println("MySql Pwd:", cfg.Section("mysql").Key("db1.Pwd").String())

	fmt.Println("Urls github:", cfg.Section("urls").Key("github").String())

	s := new(AppSettings)
	err = cfg.MapTo(s)
	if err != nil {
		fmt.Printf("Map : %v", err)
		os.Exit(1)
	}
	fmt.Println(s)

}
