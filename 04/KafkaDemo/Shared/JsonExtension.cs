namespace Shared
{
    using Newtonsoft.Json;

    public static class JsonExtension
    {
        public static string ToJsonString(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T ToObj<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}
