using System;

namespace D365WebApiClient.WebApiQueryOptions.Options
{
    /// <inheritdoc />
    /// <summary>
    /// 限制结果
    /// <para>您不应将 $top 与 $count 一起使用</para>
    /// <para>https://msdn.microsoft.com/zh-cn/library/gg334767.aspx#限制结果</para>
    /// </summary>
    public class QueryTop : QueryOption
    {
        /// <summary>
        /// 限制结果
        /// </summary>
        public QueryTop()
        {

        }

        /// <summary>
        /// 限制结果
        /// </summary>
        /// <param name="top">返回结果</param>
        public QueryTop(int top)
        {
            if (top < 0)
            {
                throw new ArgumentException("不能小于0", nameof(top));
            }
            this.Top = top;
        }

        public override string OptionName => Name;

        public const string Name = "$top";

        /// <summary>
        /// 返回结果
        /// </summary>
        public int Top { get; set; }

        public override string Builder()
        {
            return $"{OptionName}={Top}";
        }

    }
}
