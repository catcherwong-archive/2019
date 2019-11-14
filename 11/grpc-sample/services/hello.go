package services

import (
	"context"
	pb "grpc-sample/protos"
	"log"
)

func (s *server) SayHello(ctx context.Context, in *pb.HelloRequest) (*pb.HelloReply, error) {
	log.Println("request: ", in.Name)
	return &pb.HelloReply{Message: "hello, " + in.Name}, nil
}
