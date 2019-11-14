package main

import (
	"google.golang.org/grpc"
	pb "grpc-sample/protos"
	"grpc-sample/services"
	"log"
	"net"
)

const (
	PORT = ":9192"
)

func main() {
	lis, err := net.Listen("tcp", PORT)

	if err != nil {
		log.Fatalf("failed to listen: %v", err)
	}

	s := grpc.NewServer()
	pb.RegisterGreeterServer(s, services.NewServer())
	log.Println("rpc services started, listen on localhost:9192")
	s.Serve(lis)
}
