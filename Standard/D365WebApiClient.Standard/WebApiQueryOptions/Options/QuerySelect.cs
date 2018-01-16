using System.Linq;

namespace D365WebApiClient.Standard.WebApiQueryOptions.Options
{
    /// <inheritdoc />
    /// <summary>
    /// 请求特定属性
    /// <para>https://msdn.microsoft.com/zh-cn/library/gg334767.aspx#请求特定属性</para>
    /// </summary>
    public class QuerySelect : QueryOption
    {
        /// <summary>
        /// 请求特定属性
        /// </summary>
        public QuerySelect()
        {
            
        }

        /// <summary>
        /// 请求特定属性
        /// </summary>
        /// <param name="allcolumns">所有</param>
        public QuerySelect(bool allcolumns = true)
        {
            AllColumns = allcolumns;
        }

        /// <summary>
        /// 请求特定属性
        /// </summary>
        /// <param name="columns">属性</param>
        public QuerySelect(params string[] columns)
        {
            if (columns != null)
                this.Columns = columns.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        }

        public override string OptionName => "$select";

        /// <summary>
        /// 查询列
        /// </summary>
        public string[] Columns { get; set; }

        /// <summary>
        /// 所有列
        /// </summary>
        public bool AllColumns { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// 查询字符
        /// </summary>
        /// <returns></returns>
        public override string Builder()
        {
            if (AllColumns)
            {
                return string.Empty;
            }

            if (Columns == null)
            {
                return string.Empty;
            }
            if (Columns.Length == 0)
            {
                //throw new Exception("参数至少1个");
                return string.Empty;
            }

            var colums = string.Join(",", Columns);
            return $"{OptionName}={colums}";
        }

    }
}
