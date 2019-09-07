package config

import "flag"

var env = flag.String("env", "dev", "app mode")

type AppSettings struct {
	AppMode string        `ini:"app_mode" yaml:"app_mode"`
	Mysql   MySqlSettings `ini:"mysql" yaml:"mysql"`
	Url     UrlSettings   `ini:"urls" yaml:"urls"`
}

type MySqlSettings struct {
	Name string `ini:"db1.Name" yaml:"db1.name"`
	Host string `ini:"db1.Host" yaml:"db1.host"`
	Port int    `ini:"db1.Port" yaml:"db1.port"`
	User string `ini:"db1.User" yaml:"db1.user"`
	Pwd  string `ini:"db1.Pwd" yaml:"db1.pwd"`
}

type UrlSettings struct {
	Cnblogs      string `ini:"cnblogs" yaml:"cnblogs"`
	CSharpCorner string `ini:"csharpcorner" yaml:"csharpcorner"`
	Github       string `ini:"github" yaml:"github"`
}
