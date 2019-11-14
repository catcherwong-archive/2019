package main

import (
	"log"
	"net/http"

	"github.com/golang/glog"
	"github.com/grpc-ecosystem/grpc-gateway/runtime"
	"golang.org/x/net/context"
	"google.golang.org/grpc"
	gw "grpc-sample/protos"
)

func run() error {
	ctx := context.Background()
	ctx, cancel := context.WithCancel(ctx)
	defer cancel()

	mux := runtime.NewServeMux()

	opts := []grpc.DialOption{grpc.WithInsecure()}

	err := gw.RegisterGreeterHandlerFromEndpoint(ctx, mux, ":9192", opts)
	if err != nil {
		return err
	}

	log.Println("grpc-gateway listen on localhost:8080")
	return http.ListenAndServe(":8080", mux)
}

func main() {
	defer glog.Flush()

	if err := run(); err != nil {
		glog.Fatal(err)
	}
}
