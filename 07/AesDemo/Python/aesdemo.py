from Crypto.Cipher import AES
import base64
import hashlib


def jm_sha256(data):
    sha256 = hashlib.sha256()
    sha256.update(data.encode("utf-8"))    
    res = sha256.digest()
    # print("sha256加密结果:", res)
    return res

def pkcs7padding(text):

    bs = AES.block_size 
    length = len(text)
    bytes_length = len(bytes(text, encoding='utf-8'))

    # tips：utf-8编码时，英文占1个byte，而中文占3个byte
    padding_size = length if(bytes_length == length) else bytes_length
    padding = bs - padding_size % bs

    # tips：chr(padding)看与其它语言的约定，有的会使用'\0'
    padding_text = chr(padding) * padding
    return text + padding_text

def aes_encrypt_v2(content, key):


    key_bytes = jm_sha256(key)

    iv = "\0".encode("utf-8") * 16

    aes = AES.new(key_bytes, AES.MODE_CBC, iv)

    content_padding = pkcs7padding(content)

    encrypt_bytes = aes.encrypt(bytes(content_padding, encoding='utf-8'))

    result = str(base64.b64encode(encrypt_bytes), encoding='utf-8')
    return result


mystr1 = "123"
mykey1 = "12345678"

# 3gVLeGnili1JBTYLHAk8pQ==
print(aes_encrypt_v2(mystr1, mykey1))

mystr2 = "你好abcd1234"
mykey2 = "1234567812345678"

# Qkz+MXCIESJZVgHJffouTQ==
print(aes_encrypt_v2(mystr2, mykey2))