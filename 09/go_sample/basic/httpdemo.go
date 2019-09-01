package basic

import (
	"bytes"
	"encoding/json"
	"fmt"
	"io/ioutil"
	"net/http"
	"net/url"
)

type HttpDemo struct {
}

func ShowHttpDemo() {

	h := new(HttpDemo)

	h.simpleGet()
	h.simplePostJson()
	h.simplePostForm()
	h.httpDo()

	fmt.Println("Hello Golang.")
}

func (h HttpDemo) simpleGet() {
	resp, err := http.Get("http://localhost:8989/api/values")
	if err != nil {
		fmt.Println(err)
		return
	}

	defer resp.Body.Close()
	body, err := ioutil.ReadAll(resp.Body)
	if err != nil {
		fmt.Println(err)
		return
	}

	fmt.Println("get", string(body))
}

func (h HttpDemo) simplePostJson() {

	data := make(map[string]interface{})

	data["id"] = "1"
	data["name"] = "catcher"

	b, err := json.Marshal(data)

	resp, err := http.Post("http://localhost:8989/api/values/json",
		"application/json",
		bytes.NewBuffer(b))

	if err != nil {
		fmt.Println(err)
		return
	}

	defer resp.Body.Close()

	body, err := ioutil.ReadAll(resp.Body)
	if err != nil {
		fmt.Println(err)
		return
	}

	fmt.Println("post json", string(body))
}

func (h HttpDemo) simplePostForm() {

	resp, err := http.PostForm("http://localhost:8989/api/values/form", url.Values{"id": {"1"}, "name": {"catcher"}})

	if err != nil {
		fmt.Println(err)
		return
	}

	defer resp.Body.Close()

	body, err := ioutil.ReadAll(resp.Body)
	if err != nil {
		fmt.Println(err)
		return
	}

	fmt.Println("post form", string(body))
}

func (h HttpDemo) httpDo() {
	client := &http.Client{}

	data := make(map[string]interface{})

	data["id"] = "1"
	data["name"] = "catcher"

	b, err := json.Marshal(data)

	req, err := http.NewRequest("POST", "http://localhost:8989/api/values/json", bytes.NewBuffer(b))
	if err != nil {
		fmt.Println(err)
		return
	}

	req.Header.Set("Content-Type", "application/json")

	resp, err := client.Do(req)

	if err != nil {
		fmt.Println(err)
		return
	}

	defer resp.Body.Close()

	body, err := ioutil.ReadAll(resp.Body)
	if err != nil {
		fmt.Println(err)
		return
	}

	fmt.Println("http do", string(body))
}
