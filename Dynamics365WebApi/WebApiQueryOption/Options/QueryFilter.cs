using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynamics365WebApi.WebApiQueryOption.Options.Filter;

namespace Dynamics365WebApi.WebApiQueryOption.Options
{
    /// <summary>
    /// 筛选结果
    /// <para>https://msdn.microsoft.com/zh-cn/library/gg334767.aspx#筛选结果</para>
    /// </summary>
    public class QueryFilter : QueryOption
    {
        public QueryFilter(FilterExpression filterExpression)
        {
            if (filterExpression == null)
            {
                throw new ArgumentNullException(nameof(filterExpression));
            }
            FilterExpression = filterExpression;
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

        public override string ToString()
        {
            return Builder();
        }

    }
}
