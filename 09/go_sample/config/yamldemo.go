package config

import (
	"fmt"

	"flag"
	"io/ioutil"

	"github.com/go-yaml/yaml"
)

func ShowYaml() {

	flag.Parse()

	fmt.Println(*env)

	f := fmt.Sprintf("config/demo.%s.yaml", *env)

	var setting AppSettings
	config, err := ioutil.ReadFile(f)
	if err != nil {
		fmt.Print(err)
	}
	yaml.Unmarshal(config, &setting)

	fmt.Println(setting)
}
