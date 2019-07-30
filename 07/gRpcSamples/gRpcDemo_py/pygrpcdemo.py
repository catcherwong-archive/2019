import grpc
import userinfo_pb2, userinfo_pb2_grpc

_HOST = 'localhost'
_PORT = '9999'

def run():
    conn = grpc.insecure_channel(_HOST + ':' + _PORT)

    client = userinfo_pb2_grpc.UserInfoServiceStub(channel=conn)

    saveResponse = client.Save(userinfo_pb2.SaveUserRequest(name="pyname", age=39))
    print("Save received: code = " + str(saveResponse.code) + ", msg = "+ saveResponse.msg)

    getListResponse = client.GetList(userinfo_pb2.GetUserListRequest(id=1, name="aa"))
    print("GetList received: code = " + str(getListResponse.code) + ", msg = "+ getListResponse.msg)

    for d in getListResponse.data:
        print(d.name)

if __name__ == '__main__':
    run()