module grpc-sample

go 1.13

require (
	github.com/golang/protobuf v1.3.2
	github.com/grpc-ecosystem/grpc-gateway v1.12.1
	google.golang.org/genproto v0.0.0-20191108220845-16a3f7862a1a
	google.golang.org/grpc v1.25.1
	gopkg.in/yaml.v2 v2.2.5 // indirect
)

replace google.golang.org/grpc => github.com/grpc/grpc-go v1.25.1

replace google.golang.org/genproto => github.com/google/go-genproto v0.0.0-20191108220845-16a3f7862a1a
