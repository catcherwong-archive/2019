namespace ConsoleApp1
{
    using System;
    using System.Text;
    using System.Security.Cryptography;

    class Program
    {
        static void Main(string[] args)
        {
            var key1 = "12345678";
            var str1 = "123";            
            var res1 = AesEncrypt(str1, key1);

            //3gVLeGnili1JBTYLHAk8pQ==
            Console.WriteLine(res1);
            
            var key2 = "1234567812345678";
            var str2 = "你好abcd1234";
            var res2 = AesEncrypt(str2, key2);

            //Qkz+MXCIESJZVgHJffouTQ==
            Console.WriteLine(res2);

            Console.ReadKey();
        }

        public static string AesEncrypt(string toEncrypt, string key)
        {
            byte[] keyArray = SHA256(key);
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
            byte[] iv = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, };

            RijndaelManaged rDel = new RijndaelManaged
            {
                Key = keyArray,
                IV = iv,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                BlockSize = 128
            };

            ICryptoTransform cTransform = rDel.CreateEncryptor();

            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static byte[] SHA256(string str)
        {
            byte[] SHA256Data = Encoding.UTF8.GetBytes(str);
            SHA256Managed Sha256 = new SHA256Managed();
            byte[] by = Sha256.ComputeHash(SHA256Data);
            return by;
        }
    }
}
