namespace D365WebApiClient.WebApiQueryOption.Options.Filter
{
    /// <summary>
    /// 比较运算符
    /// </summary>
    public enum ConditionOperator
    {
        //Comparison Operators
        /// <summary>
        /// eq 等于
        /// </summary>
        Equal,
        /// <summary>
        /// ne 不等于
        /// </summary>
        NotEqual,
        /// <summary>
        /// gt 大于
        /// </summary>
        GreaterThan,
        /// <summary>
        /// ge 大于或等于
        /// </summary>
        GreaterThanOrQqual,
        /// <summary>
        /// lt 小于
        /// </summary>
        LessThan,
        /// <summary>
        /// le 小于或等于
        /// </summary>
        LessThanOrQqual,
        /// <summary>
        /// startswith
        /// </summary>
        StartsWith,
        /// <summary>
        /// endswith
        /// </summary>
        EndsWith,
        /// <summary>
        /// contains
        /// </summary>
        Contains
    }
}
