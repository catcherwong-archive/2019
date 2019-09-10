package logdemo

import (
	"log"

	"os"
)

func ShowBasicLog() {

	log.Print("abc")
	log.Printf("I love %s, %d", "you", 111)
	log.Println("test println")

	// log.Fatal("abc")

	debugLog := log.New(os.Stdout, "DEBUG ", log.Ldate|log.Lmicroseconds)
	infoLog := log.New(os.Stdout, "INFO  ", log.Ldate|log.Lmicroseconds)

	debugLog.Println("debug log")
	infoLog.Println("info log")

}
