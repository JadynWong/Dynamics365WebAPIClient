using System;

namespace D365WebApiClient.Standard.WebApiQueryOptions.Options
{
    /// <inheritdoc />
    /// <summary>
    /// 排序结果
    /// <para>https://msdn.microsoft.com/zh-cn/library/gg334767.aspx#排序结果</para>
    /// </summary>
    public class QueryOrderBy : QueryOption
    {
        /// <summary>
        /// 排序结果 默认
        /// </summary>
        public QueryOrderBy()
        {

        }

        /// <summary>
        /// 排序结果 默认
        /// </summary>
        /// <param name="descend"></param>
        /// <param name="columns"></param>
        public QueryOrderBy(bool descend, params string[] columns)
        {
            Descend = descend;
            if (columns == null)
            {
                throw new ArgumentNullException(nameof(columns));
            }
            Columns = columns;
        }

        /// <summary>
        /// 排序结果 默认
        /// </summary>
        /// <param name="columns"></param>
        public QueryOrderBy(params string[] columns)
        {
            if (columns == null)
            {
                throw new ArgumentNullException(nameof(columns));
            }
            Columns = columns;
        }

        public override string OptionName => "$orderby";

        /// <summary>
        /// 查询列
        /// </summary>
        public string[] Columns { get; set; }

        /// <summary>
        /// False 
        /// <para>默认升序asc</para>
        /// <para>False 正序 asc</para>
        /// <para>True 倒序 desc</para>
        /// </summary>
        public bool? Descend { get; set; }

        public override string Builder()
        {
            if (Columns.Length == 0)
            {
                throw new ArgumentException("必须包含排序列", nameof(Columns));
            }

            var colums = string.Join(",", Columns);
            var orderBy = $"{OptionName}={colums}";
            if (Descend.HasValue)
            {
                if (Descend.Value)
                {
                    orderBy += " desc";
                }
                else
                {
                    orderBy += " asc";
                }
            }

            return orderBy;
        }
    }
}
