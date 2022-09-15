using Core.ElasticSearch.Models;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Nest;
using Nest.JsonNetSerializer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ElasticSearch
{
    public class ElasticSearchManager : IElasticSearch
    {
        private readonly ConnectionSettings _conncetionSettings;

        public ElasticSearchManager(IConfiguration configuration)
        {
            ElasticSearchConfiguration? settings = configuration.GetSection("ElasticSearchConfiguration").Get<ElasticSearchConfiguration>();
            SingleNodeConnectionPool pool = new(new Uri(settings.ConnectionString));
            _conncetionSettings = new ConnectionSettings(pool, (builtInSerializer, connectionSettings) =>
                            new JsonNetSerializer(builtInSerializer, connectionSettings, () =>
                                new JsonSerializerSettings
                                {
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                }));
        }
        public async Task<IElasticSearchResult> CreateNewIndexAsync(IndexModel indexModel)
        {
            ElasticClient elasticClient = GetElasticClient(indexModel.IndexName);
            if (elasticClient.Indices.Exists(indexModel.IndexName).Exists)
                return new ElasticSearchResult(false, "Index already exists");

            CreateIndexResponse? response = await elasticClient.Indices.CreateAsync(indexModel.IndexName, (s) =>
                s.Settings(x => x.NumberOfReplicas(indexModel.NumberOfReplicas).NumberOfShards(indexModel.NumberOfShards)).Aliases(x => x.Alias(indexModel.AliasName)));

            return new ElasticSearchResult(response.IsValid, response.IsValid ? "Success" : response.ServerError.Error.Reason);
        }

        public async Task<IElasticSearchResult> DeleteByElasticIdAsync(ElasticSearchModel model)
        {
            ElasticClient elasticClient = GetElasticClient(model.IndexName);
            DeleteResponse? response = await elasticClient.DeleteAsync<object>(model.ElasticId, (s) => s.Index(model.IndexName));

            return new ElasticSearchResult(response.IsValid, response.IsValid ? "Success" : response.ServerError.Error.Reason);
        }

        public async Task<List<ElasticSearchGetModel<T>>> GetAllSearch<T>(SearchParameters searchParameters) where T : class
        {
            Type type = typeof(T);

            ElasticClient elasticClient = GetElasticClient(searchParameters.IndexName);
            ISearchResponse<T>? searchResponse = await elasticClient.SearchAsync<T>(s => s
                        .Index(Indices.Index(searchParameters.IndexName))
                        .From(searchParameters.From)
                        .Size(searchParameters.Size));

            List<ElasticSearchGetModel<T>> list = searchResponse.Hits.Select(x => new ElasticSearchGetModel<T>()
            {
                ElasticId = x.Id,
                Item = x.Source
            }).ToList();

            return list;
        }

        public IReadOnlyDictionary<IndexName, IndexState> GetIndexList()
        {
            ElasticClient elasticClient = new(_conncetionSettings);
            return elasticClient.Indices.Get(new GetIndexRequest(Indices.All)).Indices;
        }

        public async Task<List<ElasticSearchGetModel<TItem>>> GetSearchByField<TItem>(SearchByFieldParameters fieldParameters) where TItem : class
        {
            ElasticClient elasticClient = GetElasticClient(fieldParameters.IndexName);
            ISearchResponse<TItem>? searchResponse = await elasticClient.SearchAsync<TItem>(s => s
                        .Index(fieldParameters.IndexName)
                        .From(fieldParameters.From)
                        .Size(fieldParameters.Size));

            List<ElasticSearchGetModel<TItem>> list = searchResponse.Hits.Select(x => new ElasticSearchGetModel<TItem>()
            {
                ElasticId = x.Id,
                Item = x.Source
            }).ToList();

            return list;
        }

        public async Task<List<ElasticSearchGetModel<T>>> GetSearchBySimpleQueryString<T>(SearchByQueryParameters queryParameters) where T : class
        {
            ElasticClient elasticClient = GetElasticClient(queryParameters.IndexName);
            ISearchResponse<T>? searchResponse = await elasticClient.SearchAsync<T>(s => s
                         .Index(queryParameters.IndexName)
                         .From(queryParameters.From)
                         .Size(queryParameters.Size)
                         .MatchAll()
                         .Query(q => q.SimpleQueryString(s => s
                                        .Name(queryParameters.QueryName)
                                        .Boost(1.1)
                                        .Fields(queryParameters.Fields)
                                        .Query(queryParameters.Query)
                                        .Analyzer("standard")
                                        .DefaultOperator(Operator.Or)
                                        .Flags(SimpleQueryStringFlags.And | SimpleQueryStringFlags.Near)
                                        .Lenient()
                                        .AnalyzeWildcard(false)
                                        .MinimumShouldMatch("30%")
                                        .FuzzyPrefixLength(0)
                                        .FuzzyMaxExpansions(50)
                                        .FuzzyTranspositions()
                                        .AutoGenerateSynonymsPhraseQuery(false)
                                        ))
                         );

            List<ElasticSearchGetModel<T>> list = searchResponse.Hits.Select(x => new ElasticSearchGetModel<T>
            {
                ElasticId = x.Id,
                Item = x.Source
            }).ToList();

            return list;
        }

        public async Task<IElasticSearchResult> InsertManyAsync(string indexName, object[] items)
        {
            ElasticClient elasticClient = GetElasticClient(indexName);
            BulkResponse? response = await elasticClient.BulkAsync(b => b.Index(indexName).IndexMany(items));

            return new ElasticSearchResult(response.IsValid, response.IsValid ? "Success" : response.ServerError.Error.Reason);
        }

        public async Task<IElasticSearchResult> InsertAsync(ElasticSearchInsertUpdateModel model)
        {
            ElasticClient elasticClient = GetElasticClient(model.IndexName);

            IndexResponse? response = await elasticClient.IndexAsync(model.Item, i => i.Index(model.IndexName).Id(model.ElasticId).Refresh(Refresh.True));

            return new ElasticSearchResult(response.IsValid, response.IsValid ? "Success" : response.ServerError.Error.Reason);
        }

        public async Task<IElasticSearchResult> UpdateByElasticIdAsync(ElasticSearchInsertUpdateModel model)
        {
            ElasticClient elasticClient = GetElasticClient(model.IndexName);

            UpdateResponse<object>? response = await elasticClient.UpdateAsync<object>(model.ElasticId, s => s.Index(model.IndexName).Doc(model.Item));


            return new ElasticSearchResult(response.IsValid, response.IsValid ? "Success" : response.ServerError.Error.Reason);
        }

        private ElasticClient GetElasticClient(string indexName)
        {
            if (string.IsNullOrEmpty(indexName))
                throw new ArgumentNullException(indexName, "Index name cannot be null or empty.");

            return new ElasticClient(_conncetionSettings);
        }
    }
}
