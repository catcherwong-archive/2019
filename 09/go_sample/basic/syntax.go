package basic

import (
	"fmt"

	"strconv"
)

type BasicDemo struct {
}

func ShowBasicDemo() {

	b := new(BasicDemo)

	b.formatString()

	b.conver()

	fmt.Println("Hello Golang.")
}

func (b BasicDemo) formatString() {

	name := "catcher"
	age := 18

	fmt.Println("name=", name, ",age=", age)

	fmt.Printf("name=%s,age=%d\n", name, age)
}

func (b BasicDemo) conver() {

	num1 := 18.8
	num2 := 24

	num3 := int(num1) + num2

	println(num3)

	str1 := "str"

	str2 := str1 + strconv.Itoa(num3) + strconv.FormatInt(int64(num1), 10)

	println(str2)

	str3 := "34"
	n1, err := strconv.Atoi(str3)

	println(n1, err)

	str4 := "34a"
	n2, err := strconv.Atoi(str4)

	println(n2, err)

}
