using System;
using D365WebApiClient.WebApiQueryOptions.Options.Filter;

namespace D365WebApiClient.WebApiQueryOptions.Options
{
    /// <inheritdoc />
    /// <summary>
    /// 筛选结果
    /// <para>https://msdn.microsoft.com/zh-cn/library/gg334767.aspx#筛选结果</para>
    /// </summary>
    public class QueryFilter : QueryOption
    {
        public QueryFilter()
        {

        }

        public QueryFilter(FilterExpression filterExpression)
        {
            FilterExpression = filterExpression ?? throw new ArgumentNullException(nameof(filterExpression));
        }

        public FilterExpression FilterExpression { get; set; }

        public override string OptionName => "$filter";

        public override string Builder()
        {
            if (FilterExpression == null)
            {
                throw new ArgumentNullException(nameof(FilterExpression));
            }
            return $"{OptionName}={FilterExpression}";
        }

    }
}
