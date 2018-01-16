using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using D365WebApiClient.Standard.Common;
using D365WebApiClient.Standard.Configs;
using D365WebApiClient.Standard.WebApiQueryOptions;
using D365WebApiClient.Values;
using Newtonsoft.Json.Linq;

namespace D365WebApiClient.Standard.Services.WebApiServices
{
    public interface IApiClientService
    {
        Dynamics365Options Dynamics365Options { get; }

        Task<Value> CreateAndReadAsync(string entityName, Value value, QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<Value> CreateAndReadAsync(string entityName, Value value, string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<string> CreateAsync(string entityName, Value value);
        Task DeleteAsync(string entityName, Guid guid);
        Task DeleteAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues);
        Task DeleteAsync(string entityName, string alternateKey, string alternateValue);
        Task DeleteEntitySinglePropAsync(string entityName, Guid guid, string attribute);
        Task DeleteEntitySinglePropAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, string attribute);
        Task DeleteEntitySinglePropAsync(string entityName, string alternateKey, string alternateValue, string attribute);
        Task<Value> ExecuteAsync(HttpMethod httpMethod, string url, Value value = null);
        Task<HttpResponseMessage> ExecuteAsync(HttpRequestMessage req);
        Task<Value> ReadAsync(string entityName, EnumAnnotations enumAnnotations = EnumAnnotations.None, int? maxPageSize = null);
        Task<Value> ReadAsync(string entityName, Guid guid, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<Value> ReadAsync(string entityName, Guid guid, QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<Value> ReadAsync(string entityName, Guid guid, string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<Value> ReadAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<Value> ReadAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<Value> ReadAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<Value> ReadAsync(string entityName, QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None, int? maxPageSize = null);
        Task<Value> ReadAsync(string entityName, string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None, int? maxPageSize = null);
        Task<Value> ReadAsync(string entityName, string alternateKey, string alternateValue, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<Value> ReadAsync(string entityName, string alternateKey, string alternateValue, QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<Value> ReadAsync(string entityName, string alternateKey, string alternateValue, string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<Value> ReadEntitySingleProp(string entityName, Guid guid, string attribute, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<Value> UpdateAndReadAsync(string entityName, Guid guid, Value value, QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<Value> UpdateAndReadAsync(string entityName, Guid guid, Value value, string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<Value> UpdateAndReadAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, Value value, QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<Value> UpdateAndReadAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, Value value, string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<Value> UpdateAndReadAsync(string entityName, string alternateKey, string alternateValue, Value value, QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<Value> UpdateAndReadAsync(string entityName, string alternateKey, string alternateValue, Value value, string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task UpdateAsync(string entityName, Guid guid, Value value);
        Task UpdateAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, Value value);
        Task UpdateAsync(string entityName, string alternateKey, string alternateValue, Value value);
        Task UpdateEntitySinglePropAsync(string entityName, Guid guid, string attribute, Value value);
        Task UpdateEntitySinglePropAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, string attribute, Value value);
        Task UpdateEntitySinglePropAsync(string entityName, string alternateKey, string alternateValue, string attribute, Value value);
        Task UpdatePatchAsync(string entityName, Guid guid, Value value);
        Task UpdatePatchAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, Value value);
        Task UpdatePatchAsync(string entityName, string alternateKey, string alternateValue, Value value);
        Task<Value> UpsertAndReadAsync(string entityName, Guid guid, Value value, QueryOptions queryOptions, bool ifMatch = false, bool ifNoneMatch = false, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<Value> UpsertAndReadAsync(string entityName, Guid guid, Value value, string queryOptions, bool ifMatch = false, bool ifNoneMatch = false, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<Value> UpsertAndReadAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, Value value, QueryOptions queryOptions, bool ifMatch = false, bool ifNoneMatch = false, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<Value> UpsertAndReadAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, Value value, string queryOptions, bool ifMatch = false, bool ifNoneMatch = false, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<Value> UpsertAndReadAsync(string entityName, string alternateKey, string alternateValue, Value value, QueryOptions queryOptions, bool ifMatch = false, bool ifNoneMatch = false, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<Value> UpsertAndReadAsync(string entityName, string alternateKey, string alternateValue, Value value, string queryOptions, bool ifMatch = false, bool ifNoneMatch = false, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task UpsertAsync(string entityName, Guid guid, Value value, bool ifMatch = false, bool ifNoneMatch = false);
        Task UpsertAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, Value value, bool ifMatch = false, bool ifNoneMatch = false);
        Task UpsertAsync(string entityName, string alternateKey, string alternateValue, Value value, bool ifMatch = false, bool ifNoneMatch = false);
        Task<Version> WebApiVersion();
        Task<Value> WhoImIAsync();
    }
}