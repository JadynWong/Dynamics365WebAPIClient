using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace D365WebApiClient.Services.WebApiServices
{
    public partial class ApiClientService
    {
        /// <inheritdoc />
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string entityName, Guid guid)
        {
            var url = BuildGuidUrl(entityName, guid);

            var req = BuildRequest(HttpMethod.Delete, url);
            var response = await this.ExecuteAsync(req); //204
        }

        /// <inheritdoc />
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKey"></param>
        /// <param name="alternateValue"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string entityName, string alternateKey, string alternateValue)
        {
            await DeleteAsync(entityName, new[] { new KeyValuePair<string, string>(alternateKey, alternateValue) });
        }

        /// <inheritdoc />
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues)
        {
            var url = BuildAlternateKeyUrl(entityName, alternateKeyValues);

            var req = BuildRequest(HttpMethod.Delete, url);
            var response = await this.ExecuteAsync(req); //204
        }

        /// <inheritdoc />
        /// <summary>
        /// 删除单个属性
        /// </summary>
        /// <para>这无法用于单一值导航属性解除两个实体的关联</para>
        /// <param name="entityName"></param>
        /// <param name="guid"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public async Task DeleteEntitySinglePropAsync(string entityName, Guid guid, string attribute)
        {
            //Create unique guidentifier by appending property name 
            var url = BuildGuidUrl(entityName, guid, attribute);

            //Now update just the single property.
            var httpRequestMessage = BuildRequest(HttpMethod.Delete, url);
            var response = await this.ExecuteAsync(httpRequestMessage); //204
        }

        /// <inheritdoc />
        /// <summary>
        /// 删除单个属性
        /// </summary>
        /// <para>这无法用于单一值导航属性解除两个实体的关联</para>
        /// <param name="entityName"></param>
        /// <param name="alternateValue"></param>
        /// <param name="attribute"></param>
        /// <param name="alternateKey"></param>
        /// <returns></returns>
        public async Task DeleteEntitySinglePropAsync(string entityName, string alternateKey, string alternateValue, string attribute)
        {
            //Create unique guidentifier by appending property name 
            var url = BuildAlternateKeyUrl(entityName, new[] {new KeyValuePair<string, string>(alternateKey, alternateValue)}, attribute);

            //Now update just the single property.
            var httpRequestMessage = BuildRequest(HttpMethod.Delete, url);
            var response = await this.ExecuteAsync(httpRequestMessage); //204
        }

        /// <inheritdoc />
        /// <summary>
        /// 删除单个属性
        /// </summary>
        /// <para>这无法用于单一值导航属性解除两个实体的关联</para>
        /// <param name="entityName"></param>
        /// <param name="alternateKeyValues"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public async Task DeleteEntitySinglePropAsync(string entityName, IEnumerable<KeyValuePair<string, string>> alternateKeyValues, string attribute)
        {
            //Create unique guidentifier by appending property name 
            var url = BuildAlternateKeyUrl(entityName, alternateKeyValues, attribute);

            //Now update just the single property.
            var httpRequestMessage = BuildRequest(HttpMethod.Delete, url);
            var response = await this.ExecuteAsync(httpRequestMessage); //204
        }
    }
}
