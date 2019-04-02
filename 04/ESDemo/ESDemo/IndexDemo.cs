namespace ESDemo
{
    using Nest;
    using System;

    public class IndexDemo
    {       
        public void CreateIndex(ElasticClient client)
        {
            var createIndexRequest = new CreateIndexRequest("esdemo");

            var response = client.CreateIndex(createIndexRequest);

            var str = string.Empty;
            Console.WriteLine($" create index = {!response.TryGetServerErrorReason(out str)}");
            Console.WriteLine(str);
        }

        public void DeleteIndex(ElasticClient client)
        {
            var deleteIndexRequest = new DeleteIndexRequest(Indices.Parse("esdemo"));

            var response = client.DeleteIndex(deleteIndexRequest);

            var str = string.Empty;                        
            Console.WriteLine($" delete index = {!response.TryGetServerErrorReason(out str)}");
            Console.WriteLine(str);
        }

        public void IndexExists(ElasticClient client)
        {
            var response = client.IndexExists(Indices.Parse("esdemo"));

            Console.WriteLine($" index esdemo exists = {response.Exists}");
        }
    }
}