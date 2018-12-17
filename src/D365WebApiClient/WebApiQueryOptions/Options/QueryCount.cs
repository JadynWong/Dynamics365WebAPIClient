namespace D365WebApiClient.WebApiQueryOptions.Options
{
    /// <inheritdoc />
    /// <summary>
    /// 检索实体计数
    /// <para>您不应将 $top 与 $count 一起使用</para>
    /// <para>https://msdn.microsoft.com/zh-cn/library/gg334767.aspx#检索实体计数</para>
    /// </summary>
    public class QueryCount : QueryOption
    {
        public QueryCount()
        {

        }

        /// <summary>
        /// 检索实体计数
        /// </summary>
        /// <param name="count">是否返回实体计数</param>
        public QueryCount(bool count)
        {
            Count = count;
        }

        public override string OptionName => Name;

        public const string Name = "$count";

        /// <summary>
        /// 是否返回实体计数
        /// </summary>
        public bool Count { get; set; }

        public override string Builder()
        {
            var value = Count ? "true" : "false";
            return $"{OptionName}={value}";
        }
    }
}
