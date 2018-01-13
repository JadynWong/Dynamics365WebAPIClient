namespace D365WebApiClient.WebApiQueryOption
{
    /// <summary>
    /// 查询选项
    /// </summary>
    public abstract class QueryOption
    {
        /// <summary>
        /// 查询方法
        /// </summary>
        public abstract string OptionName { get; }

        /// <summary>
        /// 生成查询
        /// </summary>
        /// <returns></returns>
        public abstract string Builder();

        /// <summary>
        /// 实现生成查询
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Builder();
        }
    }
}
