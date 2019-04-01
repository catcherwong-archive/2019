using Nest;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ESDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var client = GetClient();

            //IndexDoc(client);

            //IndexMultiDoc(client);

            SearchDoc_ById(client);

            //SearchDoc_Term(client);

            //SearchDoc_Must(client);

            //SearchDoc_WildcardAndDataRange(client);

            Console.ReadKey();
        }

        static ElasticClient GetClient()
        {
            var node = new Uri("http://catcherwong:9200/");
            var settings = new ConnectionSettings(node).DefaultIndex("esdemo");
            var client = new ElasticClient(settings);
            return client;
        }

        static void IndexDoc(ElasticClient client)
        {
            var id = DateTimeOffset.UtcNow.Ticks;

            var p = new Product { Id = id, Category = "food", Name = "test", Description = "hello here", Price = 20, CreateTime = new DateTime(2018, 3, 3) };

            var indexResponse = client.IndexDocument(p);

            Console.WriteLine(JsonConvert.SerializeObject(indexResponse));
            Console.WriteLine("=========");
        }

        static void IndexMultiDoc(ElasticClient client)
        {
            var list = new List<Product>();

            for (int i = 0; i < 50; i++)
            {
                var id = DateTimeOffset.UtcNow.Ticks;

                list.Add(new Product
                {
                    Id = id,
                    Category = i % 2 == 0 ? "food" : "other",
                    Name = i % 2 == 0 ? $"test-{i}" : $"my-{i}",
                    Description = "hello here",
                    Price = 20,
                    CreateTime = new DateTime(2018, new Random().Next(1, 12), new Random().Next(1, 28))
                });
            }

            var indexResponse = client.IndexMany(list);

            //var request = new BulkDescriptor();
            //foreach (var entity in list)
            //{
            //    request.Index<Product>(op => op.Document(entity));
            //}
            //client.Bulk(request);

            Console.WriteLine(JsonConvert.SerializeObject(indexResponse));
            Console.WriteLine("=========");
        }

        static void SearchDoc_Term(ElasticClient client)
        {
            var searchRequest = new SearchRequest<Product>(Indices.Parse("esdemo"), Types.Parse("product"))
            {
                From = 0,
                Size = 10,
                Query = new TermQuery
                {
                    Field = Infer.Field<Product>(p => p.Category),
                    Value = "food"
                },
                Source = new SourceFilter
                {
                    Includes = new string[] { "id", "name", "category", "price" },
                    Excludes = new string[] { "description", "createTime" }
                }
            };

            // select p.id,p.name,p.category,p.price 
            // from esdemo.product p 
            // where p.category = 'food' 
            // limit 0,10

            var searchResponse = client.Search<Product>(searchRequest);

            var list = searchResponse.HitsMetadata.Hits.Select(x => x.Source).Select(x => GetProductVm(x)).ToList();
            var list2 = searchResponse.Documents.Select(x => GetProductVm(x)).ToList();
            var list3 = searchResponse.Hits.Select(x => x.Source).Select(x => GetProductVm(x)).ToList();

            var count = searchResponse.HitsMetadata.Total;
            var count2 = searchResponse.Total;

            Console.WriteLine("=========");
        }

        static void SearchDoc_Must(ElasticClient client)
        {
            var searchRequest = new SearchRequest<Product>(Indices.Parse("esdemo"), Types.Parse("product"))
            {
                From = 0,
                Size = 10,
                Query = new MatchQuery
                {
                    Field = Infer.Field<Product>(p => p.Description),
                    Query = "hello her",                    
                    //MinimumShouldMatch = new MinimumShouldMatch(2)
                }
            };
          
            var searchResponse = client.Search<Product>(searchRequest);
            
            var list = searchResponse.Documents;
            
            var count = searchResponse.Total;

            Console.WriteLine("=========");
        }

        static void SearchDoc_ById(ElasticClient client)
        {
            var getRequest = new GetRequest<Product>("esdemo", "product", 636897024009592378);

            var getResponse = client.Get<Product>(getRequest);

            // select * from esdemo.product p 
            // where p.id = '636897024009592378'
            // limit 1

            var p = getResponse.Source;
            
            Console.WriteLine("=========");
        }

        static void SearchDoc_WildcardAndDataRange(ElasticClient client)
        {
            var searchRequest = new SearchRequest<Product>(Indices.Parse("esdemo"), Types.Parse("product"))
            {
                From = 0,
                Size = 10,
                Query =
                new WildcardQuery
                {
                    Field = Infer.Field<Product>(p => p.Category),
                    Value = "*oo*",
                }
                |
                new DateRangeQuery
                {
                    Field = Infer.Field<Product>(p => p.CreateTime),
                    GreaterThanOrEqualTo = new DateTime(2018, 3, 1),
                    LessThanOrEqualTo = new DateTime(2018, 3, 8),
                }
            };

            // select * from esdemo.product p 
            // where p.name like '%oo%' 
            // and p.createtime >= '2018-03-01' and p.createtime <= '2018-03-08'
            // limit 0,10 
            
            var searchResponse = client.Search<Product>(searchRequest);

            var hits = searchResponse.Hits;
            var total = searchResponse.Total;

            Console.WriteLine(JsonConvert.SerializeObject(searchResponse));
            Console.WriteLine("=========");
        }

        static ProductVm GetProductVm(Product product)
        {
            return new ProductVm
            {
                Category = product.Category,
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
        }
    }
}
