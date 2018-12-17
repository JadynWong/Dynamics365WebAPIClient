using System;

namespace D365WebApiClient.WebApiQueryOptions.Options
{
    /// <inheritdoc />
    /// <summary>
    /// 已保存查询 预定义的查询
    /// <para>https://msdn.microsoft.com/zh-cn/library/mt607533.aspx#预定义的查询</para>
    /// </summary>
    public class QuerySavedQuery : QueryOption
    {
        public QuerySavedQuery(Guid savedQueryId)
        {
            SavedQueryId = savedQueryId;
        }

        public override string OptionName => "savedQuery";

        public Guid SavedQueryId { get; set; }

        public override string Builder()
        {
            return $"{OptionName}={SavedQueryId:D}";
        }
    }
}