using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using D365WebApiClient.Standard.Common;
using D365WebApiClient.Standard.Configs;
using D365WebApiClient.Standard.WebApiQueryOptions;
using Newtonsoft.Json.Linq;

namespace D365WebApiClient.Standard.Services.WebApiServices
{
    public interface IApiClientService
    {
        Dynamics365Options Dynamics365Options { get; }

        Task<JObject> CreateAndReadAsync(string entityName, JObject jObject, QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<JObject> CreateAndReadAsync(string entityName, JObject jObject, string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<string> CreateAsync(string entityName, JObject jObject);
        Task DeleteAsync(string entityName, Guid guid);
        Task DeleteAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues);
        Task DeleteAsync(string entityName, string alternateKey, string alternateValue);
        Task DeleteEntitySinglePropAsync(string entityName, Guid guid, string attribute);
        Task DeleteEntitySinglePropAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, string attribute);
        Task DeleteEntitySinglePropAsync(string entityName, string alternateKey, string alternateValue, string attribute);
        Task<JObject> ExecuteAsync(HttpMethod httpMethod, string url, JObject jObject = null);
        Task<HttpResponseMessage> ExecuteAsync(HttpRequestMessage req);
        Task<JObject> ReadAsync(string entityName, EnumAnnotations enumAnnotations = EnumAnnotations.None, int? maxPageSize = null);
        Task<JObject> ReadAsync(string entityName, Guid guid, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<JObject> ReadAsync(string entityName, Guid guid, QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<JObject> ReadAsync(string entityName, Guid guid, string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<JObject> ReadAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<JObject> ReadAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<JObject> ReadAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<JObject> ReadAsync(string entityName, QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None, int? maxPageSize = null);
        Task<JObject> ReadAsync(string entityName, string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None, int? maxPageSize = null);
        Task<JObject> ReadAsync(string entityName, string alternateKey, string alternateValue, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<JObject> ReadAsync(string entityName, string alternateKey, string alternateValue, QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<JObject> ReadAsync(string entityName, string alternateKey, string alternateValue, string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<JObject> ReadEntitySingleProp(string entityName, Guid guid, string attribute, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<JObject> UpdateAndReadAsync(string entityName, Guid guid, JObject jObject, QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<JObject> UpdateAndReadAsync(string entityName, Guid guid, JObject jObject, string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<JObject> UpdateAndReadAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, JObject jObject, QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<JObject> UpdateAndReadAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, JObject jObject, string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<JObject> UpdateAndReadAsync(string entityName, string alternateKey, string alternateValue, JObject jObject, QueryOptions queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<JObject> UpdateAndReadAsync(string entityName, string alternateKey, string alternateValue, JObject jObject, string queryOptions, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task UpdateAsync(string entityName, Guid guid, JObject jObject);
        Task UpdateAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, JObject jObject);
        Task UpdateAsync(string entityName, string alternateKey, string alternateValue, JObject jObject);
        Task UpdateEntitySinglePropAsync(string entityName, Guid guid, string attribute, JObject jObject);
        Task UpdateEntitySinglePropAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, string attribute, JObject jObject);
        Task UpdateEntitySinglePropAsync(string entityName, string alternateKey, string alternateValue, string attribute, JObject jObject);
        Task UpdatePatchAsync(string entityName, Guid guid, JObject jObject);
        Task UpdatePatchAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, JObject jObject);
        Task UpdatePatchAsync(string entityName, string alternateKey, string alternateValue, JObject jObject);
        Task<JObject> UpsertAndReadAsync(string entityName, Guid guid, JObject jObject, QueryOptions queryOptions, bool ifMatch = false, bool ifNoneMatch = false, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<JObject> UpsertAndReadAsync(string entityName, Guid guid, JObject jObject, string queryOptions, bool ifMatch = false, bool ifNoneMatch = false, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<JObject> UpsertAndReadAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, JObject jObject, QueryOptions queryOptions, bool ifMatch = false, bool ifNoneMatch = false, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<JObject> UpsertAndReadAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, JObject jObject, string queryOptions, bool ifMatch = false, bool ifNoneMatch = false, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<JObject> UpsertAndReadAsync(string entityName, string alternateKey, string alternateValue, JObject jObject, QueryOptions queryOptions, bool ifMatch = false, bool ifNoneMatch = false, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task<JObject> UpsertAndReadAsync(string entityName, string alternateKey, string alternateValue, JObject jObject, string queryOptions, bool ifMatch = false, bool ifNoneMatch = false, EnumAnnotations enumAnnotations = EnumAnnotations.None);
        Task UpsertAsync(string entityName, Guid guid, JObject jObject, bool ifMatch = false, bool ifNoneMatch = false);
        Task UpsertAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, JObject jObject, bool ifMatch = false, bool ifNoneMatch = false);
        Task UpsertAsync(string entityName, string alternateKey, string alternateValue, JObject jObject, bool ifMatch = false, bool ifNoneMatch = false);
        Task<Version> WebApiVersion();
        Task<JObject> WhoImIAsync();
    }
}