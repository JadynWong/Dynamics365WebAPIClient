using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics365WebApi.WebApiQueryOption
{
    /// <summary>
    /// 查询选项
    /// </summary>
    public class QueryOptions : List<QueryOption>, IList<QueryOption>
    {

        public QueryOptions(params QueryOption[] queryOptions)
        {
            this.AddRange(queryOptions);
        }

        /// <summary>
        /// 生成查询字符
        /// </summary>
        /// <returns></returns>
        public string Builder()
        {
            if (this.Count == 0)
            {
                return null;
            }

            var queryOptions = new StringBuilder();
            var queryOptionNames = new List<string>();
            foreach (var queryOption in this)
            {
                if (queryOption == null)
                {
                    continue;
                }
                if (queryOptionNames.Contains(queryOption.OptionName))
                {
                    throw new Exception($"参数'{queryOption.OptionName}'出现多次,参数仅能出现一次");
                }
                queryOptionNames.Add(queryOption.OptionName);
                var queryOptionStr = queryOption.Builder();
                if (string.IsNullOrWhiteSpace(queryOptionStr))
                    continue;
                if (queryOptions.Length > 0)
                {
                    queryOptions.Append("&");
                }
                queryOptions.Append(queryOptionStr);
            }

            return queryOptions.ToString();
        }

        public override string ToString()
        {
            return Builder();
        }
    }
}
