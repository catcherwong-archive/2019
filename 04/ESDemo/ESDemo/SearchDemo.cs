namespace ESDemo
{
    using Nest;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SearchDemo
    {
        public void SearchDoc_Term(ElasticClient client)
        {
            var termQuery = new TermQuery
            {
                Field = Infer.Field<Product>(p => p.Category),
                Value = "food"
            };

            var searchRequest = new SearchRequest<Product>(Indices.Parse("esdemo"), Types.Parse("product"))
            {
                From = 0,
                Size = 10,
                Query = termQuery,
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

        public void SearchDoc_Match(ElasticClient client)
        {
            var matchQuery = new MatchQuery
            {
                Field = Infer.Field<Product>(p => p.Name),
                Query = "apple",
            };

            var searchRequest = new SearchRequest<Product>(Indices.Parse("esdemo"), Types.Parse("product"))
            {
                From = 0,
                Size = 10,
                Query = matchQuery
            };

            var searchResponse = client.Search<Product>(searchRequest);

            var list = searchResponse.Documents;
            var count = searchResponse.Total;
            System.Console.WriteLine("MatchQuery");
            System.Console.WriteLine(list.ToJsonString());
            System.Console.WriteLine(count);
        }

        public void SearchDoc_CommonTerms(ElasticClient client)
        {
            var commonTermsQuery = new CommonTermsQuery()
            {
                Field = Infer.Field<Product>(p => p.Description),
                Analyzer = "standard",
                Boost = 1.1,
                CutoffFrequency = 0.001,
                HighFrequencyOperator = Operator.And,
                LowFrequencyOperator = Operator.Or,
                MinimumShouldMatch = 1,
                Name = "named_query",
                Query = "hello there"
            };

            var searchRequest = new SearchRequest<Product>(Indices.Parse("esdemo"), Types.Parse("product"))
            {
                From = 0,
                Size = 10,
                Query = commonTermsQuery
            };

            var searchResponse = client.Search<Product>(searchRequest);

            var list = searchResponse.Documents.ToList();
            var count = searchResponse.Total;
            System.Console.WriteLine("CommonTermsQuery");
            System.Console.WriteLine(list.ToJsonString());
            System.Console.WriteLine(count);
        }

        public void SearchDoc_QueryString(ElasticClient client)
        {
            var queryStringQuery = new QueryStringQuery()
            {
                Fields = Infer.Field<Product>(p => p.Description).And("name"),                
                Query = "dog",
                //Query = "test"
            };

            var searchRequest = new SearchRequest<Product>(Indices.Parse("esdemo"), Types.Parse("product"))
            {
                From = 0,
                Size = 10,
                Query = queryStringQuery
            };

            var searchResponse = client.Search<Product>(searchRequest);

            var list = searchResponse.Documents.ToList();
            var count = searchResponse.Total;
            System.Console.WriteLine("QueryStringQuery");
            System.Console.WriteLine(list.ToJsonString());
            System.Console.WriteLine(count);
        }

        public void SearchDoc_SimpleQueryString(ElasticClient client)
        {
            var simpleQueryStringQuery = new SimpleQueryStringQuery()
            {
                Name = "named_query",
                Boost = 1.1,
                Fields =  Infer.Field<Product>(p => p.Description).And("name"),
                Query = "apple",
                Analyzer = "standard",
                DefaultOperator = Operator.Or,
                Flags = SimpleQueryStringFlags.And | SimpleQueryStringFlags.Near,
                Lenient = true,
                AnalyzeWildcard = true,
                MinimumShouldMatch = "30%"
            };

            var searchRequest = new SearchRequest<Product>(Indices.Parse("esdemo"), Types.Parse("product"))
            {
                From = 0,
                Size = 10,
                Query = simpleQueryStringQuery
            };

            var searchResponse = client.Search<Product>(searchRequest);

            var list = searchResponse.Documents.ToList();
            var count = searchResponse.Total;
            System.Console.WriteLine("SimpleQueryStringQuery");
            System.Console.WriteLine(list.ToJsonString());
            System.Console.WriteLine(count);
        }
        
        public void SearchDoc_DateRange(ElasticClient client)
        {
            var dateRangeQuery = new DateRangeQuery
            {
                Name = "named_query",
                Boost = 1.1,
                Field = Infer.Field<Product>(p => p.CreateTime),
                GreaterThan = new DateTime(2018, 3, 5),
                LessThan = new DateTime(2018, 9, 18)                 
            };

            var searchRequest = new SearchRequest<Product>(Indices.Parse("esdemo"), Types.Parse("product"))
            {
                From = 0,
                Size = 10,
                Query = dateRangeQuery
            };

            var searchResponse = client.Search<Product>(searchRequest);

            var list = searchResponse.Documents.ToList();
            var count = searchResponse.Total;
            System.Console.WriteLine("DateRangeQuery");
            System.Console.WriteLine(list.ToJsonString());
            System.Console.WriteLine(count);
        }

        public void SearchDoc_NumericRange(ElasticClient client)
        {
            var numericRangeQuery = new NumericRangeQuery
            {
                Name = "named_query",
                Boost = 1.1,
                Field = Infer.Field<Product>(p => p.Price),
                GreaterThan = 18,
                LessThan = 25  
            };

            var searchRequest = new SearchRequest<Product>(Indices.Parse("esdemo"), Types.Parse("product"))
            {
                From = 0,
                Size = 10,
                Query = numericRangeQuery
            };

            var searchResponse = client.Search<Product>(searchRequest);

            var list = searchResponse.Documents.ToList();
            var count = searchResponse.Total;
            System.Console.WriteLine("NumericRangeQuery");
            System.Console.WriteLine(list.ToJsonString());
            System.Console.WriteLine(count);
        }

        public void SearchDoc_Fuzzy(ElasticClient client)
        {
            var fuzzyQuery = new FuzzyQuery
            {
                Name = "named_query",
                Boost = 1.1,
                Field = Infer.Field<Product>(p => p.Name),
                Value = "test"
            };

            var searchRequest = new SearchRequest<Product>(Indices.Parse("esdemo"), Types.Parse("product"))
            {
                From = 0,
                Size = 10,
                Query = fuzzyQuery
            };

            var searchResponse = client.Search<Product>(searchRequest);

            var list = searchResponse.Documents.ToList();
            var count = searchResponse.Total;
            System.Console.WriteLine("FuzzyQuery");
            System.Console.WriteLine(list.ToJsonString());
            System.Console.WriteLine(count);
        }
        
        public void SearchDoc_Wildcard(ElasticClient client)
        {
            var wildcardQuery = new WildcardQuery
            {
                Name = "named_query",
                Boost = 1.1,
                Field = Infer.Field<Product>(p => p.Name),
                Value = "te*"
            };

            var searchRequest = new SearchRequest<Product>(Indices.Parse("esdemo"), Types.Parse("product"))
            {
                From = 0,
                Size = 10,
                Query = wildcardQuery
            };

            var searchResponse = client.Search<Product>(searchRequest);

            var list = searchResponse.Documents.ToList();
            var count = searchResponse.Total;
            System.Console.WriteLine("WildcardQuery");
            System.Console.WriteLine(list.ToJsonString());
            System.Console.WriteLine(count);
        }

        public void SearchDoc_Prefix(ElasticClient client)
        {
            var prefixQuery = new PrefixQuery
            {
                Name = "named_query",
                Boost = 1.1,
                Field = Infer.Field<Product>(p => p.Description),
                Value = "ca"
            };

            var searchRequest = new SearchRequest<Product>(Indices.Parse("esdemo"), Types.Parse("product"))
            {
                From = 0,
                Size = 10,
                Query = prefixQuery
            };

            var searchResponse = client.Search<Product>(searchRequest);

            var list = searchResponse.Documents.ToList();
            var count = searchResponse.Total;
            System.Console.WriteLine("PrefixQuery");
            System.Console.WriteLine(list.ToJsonString());
            System.Console.WriteLine(count);
        }

        public void SearchDoc_MultiMatch(ElasticClient client)
        {
            var multiMatchQuery = new MultiMatchQuery
            {
                Fields = Infer.Field<Product>(p => p.Description).And("name"),
                Query = "dog",
                Analyzer = "standard",
                Boost = 1.1,
                Slop = 2,
                Fuzziness = Fuzziness.Auto,
                PrefixLength = 2,
                MaxExpansions = 2,
                Operator = Operator.Or,
                MinimumShouldMatch = 2,
                FuzzyRewrite = MultiTermQueryRewrite.ConstantScoreBoolean,
                TieBreaker = 1.1,
                CutoffFrequency = 0.001,
                Lenient = true,
                ZeroTermsQuery = ZeroTermsQuery.All,
                Name = "named_query"
            };

            var searchRequest = new SearchRequest<Product>(Indices.Parse("esdemo"), Types.Parse("product"))
            {
                From = 0,
                Size = 10,
                Query = multiMatchQuery
            };

            var searchResponse = client.Search<Product>(searchRequest);

            var list = searchResponse.Documents.ToList();
            var count = searchResponse.Total;
            System.Console.WriteLine("MultiMatchQuery");
            System.Console.WriteLine(list.ToJsonString());
            System.Console.WriteLine(count);
        }

        public void SearchDoc_Bool(ElasticClient client)
        {
            var boolQuery = new BoolQuery
            {                
                Name = "named_query",
            };

            boolQuery.Must = new List<QueryContainer> 
            {
                new SimpleQueryStringQuery()
                {                    
                    Fields =  Infer.Field<Product>(p => p.Description),
                    Query = "there",                    
                },
                new SimpleQueryStringQuery()
                {                    
                    Fields =  Infer.Field<Product>(p => p.Name),
                    Query = "apple",                    
                },
            };
            
            var searchRequest = new SearchRequest<Product>(Indices.Parse("esdemo"), Types.Parse("product"))
            {
                From = 0,
                Size = 10,
                Query = boolQuery
            };

            var searchResponse = client.Search<Product>(searchRequest);

            var list = searchResponse.Documents.ToList();
            var count = searchResponse.Total;
            System.Console.WriteLine("BoolQuery");
            System.Console.WriteLine(list.ToJsonString());
            System.Console.WriteLine(count);
        }

        private ProductVm GetProductVm(Product product)
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