protoc -I . --go_out=plugins=grpc:. hello.proto
protoc -I . --grpc-gateway_out=logtostderr=true:. hello.proto  
protoc -I . --swagger_out=logtostderr=true:. hello.proto  
