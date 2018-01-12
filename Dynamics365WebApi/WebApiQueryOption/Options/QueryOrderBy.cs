using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics365WebApi.WebApiQueryOption.Options
{
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
        /// <param name="orderBy"></param>
        /// <param name="columns"></param>
        public QueryOrderBy(bool orderBy,string[] columns)
        {
            OrderBy = orderBy;
            Columns = columns;
        }
        
        /// <summary>
        /// 排序结果 默认
        /// </summary>
        /// <param name="columns"></param>
        public QueryOrderBy(string[] columns)
        {
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
        public bool? OrderBy { get; set; }

        public override string Builder()
        {
            if (Columns.Length == 0)
            {
                throw new ArgumentException("必须包含排序列", nameof(Columns));
            }

            var colums = string.Join(",", Columns);
            var orderBy =  $"{OptionName}={colums}";
            if (OrderBy.HasValue)
            {
                if (OrderBy.Value)
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

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
