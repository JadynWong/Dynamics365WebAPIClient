using System;
using System.Collections.Generic;
using System.Text;

namespace D365WebApiClient.WebApiQueryOptions.Options
{
    /// <inheritdoc />
    /// <summary>
    /// 参数化查询
    /// <para>https://msdn.microsoft.com/zh-cn/library/gg334767.aspx#将参数别名与系统查询选项一起使用。</para>
    /// </summary>
    public class QueryParams : QueryOption
    {
        /// <summary>
        /// 参数化查询
        /// </summary>
        public QueryParams()
        {
            ParamDictionary = new Dictionary<string, string>();
        }

        /// <summary>
        /// 参数化查询
        /// </summary>
        public QueryParams(IList<QueryParam> queryParams)
        {
            if (queryParams != null)
            {
                foreach (var queryParam in queryParams)
                {
                    Add(queryParam);
                }
            }

        }

        /// <summary>
        /// 参数化查询
        /// </summary>
        /// <param name="paramDictionary">参数字典</param>
        public QueryParams(Dictionary<string, string> paramDictionary)
        {
            ParamDictionary = paramDictionary;
        }

        public override string OptionName => "Params";

        public Dictionary<string, string> ParamDictionary { get; set; }

        public void Add(QueryParam queryParam)
        {
            if (ParamDictionary == null)
            {
                ParamDictionary = new Dictionary<string, string>();
            }
            ParamDictionary.Add(queryParam.ParamName, queryParam.ParamValue);
        }

        public void Add(string paramName, string paramValue)
        {
            if (ParamDictionary == null)
            {
                ParamDictionary = new Dictionary<string, string>();
            }
            ParamDictionary.Add(paramName, paramValue);
        }

        public override string Builder()
        {
            if (ParamDictionary == null)
                return string.Empty;
            var stringBuilder = new StringBuilder();
            foreach (var param in ParamDictionary)
            {
                if (!param.Key.StartsWith("@p"))
                {
                    throw new ArgumentException("必须是@p{number}", nameof(param.Key));
                }
                if (string.IsNullOrWhiteSpace(param.Key))
                    continue;
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append("&");
                }
                stringBuilder.Append($"{param.Key}={param.Value}");
            }

            return stringBuilder.ToString();

        }
    }
}
