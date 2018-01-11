namespace Dynamics365WebApi.Service
{
    //[Flags]
    public enum WebApiEnumAnnotations
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 检索有关查找属性的数据
        /// </summary>
        FormattedValue = 1,
        /// <summary>
        /// 包括对实体的引用的单值导航属性的名称
        /// </summary>
        Associatednavigationproperty = 2,
        /// <summary>
        /// 查找所引用的实体的逻辑名称
        /// </summary>
        Lookuplogicalname = 4,
        /// <summary>
        /// 包含所有
        /// </summary>
        All = 8,
    }
}
