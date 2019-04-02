namespace ESDemo
{
    using System;
    using Nest;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var client = GetClient();

            // var indexDemo = new IndexDemo();
            // indexDemo.IndexExists(client);
            // indexDemo.DeleteIndex(client);
            // indexDemo.IndexExists(client);
            // indexDemo.CreateIndex(client);
            // indexDemo.IndexExists(client);

            var docDemo = new DocDemo();
            // docDemo.Index(client);
            // docDemo.Index2(client);
            // docDemo.IndexDocument(client);
            // docDemo.IndexMany(client);            
            // docDemo.Get(client);
            // docDemo.GetMany(client);
            
            var searchDemo = new SearchDemo();
            // searchDemo.SearchDoc_CommonTerms(client);
            // searchDemo.SearchDoc_QueryString(client);
            // searchDemo.SearchDoc_SimpleQueryString(client);
            // searchDemo.SearchDoc_DateRange(client);
            // searchDemo.SearchDoc_NumericRange(client);
            // searchDemo.SearchDoc_Fuzzy(client);
            // searchDemo.SearchDoc_Wildcard(client);
            // searchDemo.SearchDoc_Prefix(client);
            // searchDemo.SearchDoc_MultiMatch(client);
            // searchDemo.SearchDoc_Match(client);
            // searchDemo.SearchDoc_Term(client);
            // searchDemo.SearchDoc_Bool(client);

            Console.ReadKey();
        }

        static ElasticClient GetClient()
        {
            var node = new Uri("http://catcherwong:9200/");
            var settings = new ConnectionSettings(node);
            settings.DefaultIndex("esdemo");
            //settings.DisableDirectStreaming();
            var client = new ElasticClient(settings);            
            return client;
        }
    }
}
