# grpc-gateway sample


## How to run 

At first, start the rpc services

```
cd grpc-sample

go run main
```

```
2019/11/14 22:58:41 rpc services started, listen on localhost:9192
```


Then, start the gateway

```
cd grpc-sample/gateway

go run main
```

```
2019/11/14 22:58:45 grpc-gateway listen on localhost:8080
```


```
curl -X POST -k http://localhost:8080/hello_world -d '{"name":"catcher wong"}'

{"message":"hello, catcher wong"}
```

