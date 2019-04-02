namespace ESDemo
{
    using Newtonsoft.Json;

    public static class JsonHelper
    {
        public static string ToJsonString(this object obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            });
        }
        
    }
}