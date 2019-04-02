namespace ESDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;    
    using Nest;
    using Newtonsoft.Json;

    public class DocDemo
    {
        public void Index(ElasticClient client)
        {
            var indexRequest = new IndexRequest<Product>("esdemo", "product");

            var response = client.Index(indexRequest);

            Console.WriteLine($"{response.Index}-{response.Type}");
        }

        public void Index2(ElasticClient client)
        {
            var id = DateTimeOffset.UtcNow.Ticks;

            var p = new Product
            {
                Id = id,
                Category = "food",
                Name = "test",
                Description = "hello here",
                Price = 20,
                CreateTime = new DateTime(2018, new Random().Next(1, 12), new Random().Next(1, 28))
            };

            var response = client.Index(p, s => s.Index("esdemo"));

            System.Console.WriteLine(response.ToJsonString());

            Console.WriteLine($"{response.Index}-{response.Type}");
        }

        public void IndexDocument(ElasticClient client)
        {
            var p = new Product
            {
                Id = 1,
                Category = "food",
                Name = "test apple",
                Description = "hello there",
                Price = 20,
                CreateTime = new DateTime(2018, new Random().Next(1, 12), new Random().Next(1, 28))
            };

            var response = client.IndexDocument(p);

            System.Console.WriteLine(response.ToJsonString());

            Console.WriteLine($"{response.Index}-{response.Type}");
        }

        public void IndexMany(ElasticClient client)
        {
            var list = new List<Product>();

            for (long i = 2; i < 50; i++)
            {
                var p = new Product
                {
                    Id = i,
                    Category = i % 2 == 0 ? "food" : "other",
                    Name = i % 2 == 0 ? $"test {i}" : $"my {i}",
                    Description = i % 2 == 0 ? "hello here" : "cat dog",
                    Price = 20,
                    CreateTime = new DateTime(2018, new Random().Next(1, 12), new Random().Next(1, 28))
                };

                list.Add(p);
            }

            var response = client.IndexMany(list);
            //var response = client.IndexMany(objects: list, index: "esdemo", type: "product");

            System.Console.WriteLine(response.ToJsonString());
        }

        public void Get(ElasticClient client)
        {
            var response = client.Get<Product>(1);
            //var response = client.Get<Product>(1, s => s.Index("esdemo"));
            // var getRequest = new GetRequest("esdemo", "product", 1);
            // var response = client.Get<Product>(getRequest);

            // select * from esdemo.product p 
            // where p.id = '1'
            // limit 1

            System.Console.WriteLine($"Found = {response.Found}, {response.Source == null}");

            if (response.Source != null)
            {
                System.Console.WriteLine($"{response.Source.Id}-{response.Source.Name}-{response.Source.Category}");
            }
            else
            {
                System.Console.WriteLine(response.ToJsonString());
            }
        }

        public void GetMany(ElasticClient client)
        {
            var response = client.GetMany<Product>(new List<long> { 1, 2, 3, 1000});
            //var response = client.GetMany<Product>(new List<long> { 1, 2, 3, 1000}, "esdemo", "product");

            // select * from esdemo.product p 
            // where p.id in (1,2,3,1000)             

            var productList = response.Select(x => x.Source).Where(x => x != null).ToList();

            System.Console.WriteLine(productList.Count);
        }
    }
}