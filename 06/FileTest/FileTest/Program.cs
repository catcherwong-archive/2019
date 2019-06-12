namespace FileTest
{
    using System;
    using System.IO;
    using System.Text;
    using MessagePack;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            var key = "demo";

            var bytes = Encoding.UTF8.GetBytes("catcher wong");

            Set(key, bytes);

            Parallel.For(0, 100, x =>
            {
                if(x > 50)
                {
                    Set(key, Encoding.UTF8.GetBytes($"catcher wong - {x}"));
                }

                var tmp = Get(key);
                Console.WriteLine($"{x}-{Encoding.UTF8.GetString(tmp)}");
            });


            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }

        static void Set(string key, byte[] val)
        {
            var bytes = MessagePackSerializer.Serialize(new MyObject(val));

            using (FileStream stream = new FileStream($"{key}.txt", FileMode.OpenOrCreate, FileAccess.Write))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        static byte[] Get(string key)
        {
            using (FileStream stream = new FileStream($"{key}.txt", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var val = MessagePackSerializer.Deserialize<MyObject>(stream);
                return val.Value;
            }
        }
    }

    [MessagePackObject]
    public class MyObject
    {
        [SerializationConstructor]
        public MyObject(byte[] val)
        {
            this.Value = val;
        }

        [Key(0)]
        public byte[] Value { get; set; }
    }

}
