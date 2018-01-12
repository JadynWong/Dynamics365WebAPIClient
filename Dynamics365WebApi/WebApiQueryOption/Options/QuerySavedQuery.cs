using System;

namespace Dynamics365WebApi.WebApiQueryOption.Options
{
    /// <summary>
    /// 已保存查询 预定义的查询
    /// <para>https://msdn.microsoft.com/zh-cn/library/mt607533.aspx#%E9%A2%84%E5%AE%9A%E4%B9%89%E7%9A%84%E6%9F%A5%E8%AF%A2</para>
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

        public override string ToString()
        {
            return Builder();
        }
    }
}