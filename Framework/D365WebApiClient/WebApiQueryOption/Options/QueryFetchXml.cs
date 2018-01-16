using System;

namespace D365WebApiClient.WebApiQueryOption.Options
{
    /// <inheritdoc />
    /// <summary>
    /// 使用自定义 FetchXML
    /// <para>https://msdn.microsoft.com/zh-cn/library/mt607533.aspx#%E4%BD%BF%E7%94%A8%E8%87%AA%E5%AE%9A%E4%B9%89 FetchXML</para>
    /// </summary>
    public class QueryFetchXml : QueryOption
    {
        public QueryFetchXml()
        {
        }

        public QueryFetchXml(string fetchXml)
        {
            FetchXml = fetchXml;
        }

        public string FetchXml { get; set; }

        public override string OptionName => "$fetchXml";


        public override string Builder()
        {
            if(string.IsNullOrWhiteSpace(FetchXml))
                return String.Empty;
            return $"{OptionName}={Uri.EscapeUriString(FetchXml)}";
        }

    }
}