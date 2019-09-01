package basic

import (
	"fmt"

	"encoding/json"
)

type JsonDemo struct {
}

func ShowJsonDemo() {

	j := new(JsonDemo)

	j.enc()
	j.dec()

	fmt.Println("Hello Golang.")
}

func (j JsonDemo) enc() {
	m := map[string]string{
		"id":     "1",
		"name":   "catcher",
		"gender": "male",
	}

	if data, err := json.Marshal(m); err == nil {
		fmt.Printf("%s\n", data)
	}

	p := Person{Id: "2", Name: "wong", gender: "male"}

	if data, err := json.Marshal(p); err == nil {
		fmt.Printf("%s\n", data)
	}
}

func (j JsonDemo) dec() {

	mStr := `{"gender":"male","id":"1","name":"catcher"}`
	var m map[string]string
	err := json.Unmarshal([]byte(mStr), &m)

	if err == nil {
		fmt.Println(m)
	}

	pStr := `{"id":"2","Name":"wong"}`
	var p Person
	err = json.Unmarshal([]byte(pStr), &p)

	if err == nil {
		fmt.Println("Id =", p.Id, ",Name=", p.Name)
	}
}

type Person struct {
	Id     string `json:"id"`
	Name   string
	gender string
}
