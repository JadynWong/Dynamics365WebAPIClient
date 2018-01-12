using System;

namespace Dynamics365WebApi.WebApiQueryOption.Options
{
    /// <summary>
    /// 检索实体计数
    /// <para>您不应将 $top 与 $count 一起使用</para>
    /// <see cref="https://msdn.microsoft.com/zh-cn/library/gg334767.aspx#检索实体计数"/>
    /// </summary>
    public class QueryCount : QueryOption
    {
        /// <summary>
        /// 检索实体计数
        /// </summary>
        /// <param name="count">是否返回实体计数</param>
        public QueryCount(bool count)
        {
            Count = count;
        }

        public override string OptionName => "$count";

        /// <summary>
        /// 是否返回实体计数
        /// </summary>
        public bool Count { get; set; }

        public override string Builder()
        {
            var value = Count ? "true" : "false";
            return $"{OptionName}={value}";
        }

        public override string ToString()
        {
            return Builder();
        }
    }
}
