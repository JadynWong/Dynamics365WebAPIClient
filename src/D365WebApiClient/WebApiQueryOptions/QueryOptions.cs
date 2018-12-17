using System;
using System.Collections.Generic;
using System.Text;
using D365WebApiClient.WebApiQueryOptions.Options;

namespace D365WebApiClient.WebApiQueryOptions
{
    /// <inheritdoc />
    /// <summary>
    /// 查询选项
    /// </summary>
    public class QueryOptions : List<QueryOption>
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
                if (queryOptionNames.Contains(QueryCount.Name) && queryOptionNames.Contains(QueryTop.Name))
                {
                    throw new Exception($"您不应将 $top 与 $count 一起使用.https://msdn.microsoft.com/zh-cn/library/gg334767.aspx#%E9%99%90%E5%88%B6%E7%BB%93%E6%9E%9C");
                }
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
