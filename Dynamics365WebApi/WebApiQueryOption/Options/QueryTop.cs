using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics365WebApi.WebApiQueryOption.Options
{
    /// <summary>
    /// 限制结果
    /// <para>您不应将 $top 与 $count 一起使用</para>
    /// <see cref="https://msdn.microsoft.com/zh-cn/library/gg334767.aspx#限制结果"/>
    /// </summary>
    public class QueryTop : QueryOption
    {
        /// <summary>
        /// 限制结果
        /// </summary>
        /// <param name="top">返回结果</param>
        public QueryTop(int top)
        {
            this.Top = top;
        }

        public override string OptionName => "$top";

        /// <summary>
        /// 返回结果
        /// </summary>
        public int Top { get; set; }

        public override string Builder()
        {
            return $"{OptionName}={Top}";
        }

        public override string ToString()
        {
            return Builder();
        }
    }
}
