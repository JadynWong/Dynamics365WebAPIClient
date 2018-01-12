using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics365WebApi.WebApiQueryOption.Options
{
    /// <summary>
    /// 参数化查询
    /// <para>https://msdn.microsoft.com/zh-cn/library/gg334767.aspx#将参数别名与系统查询选项一起使用。</para>
    /// </summary>
    public class QueryParam : QueryOption
    {
        /// <summary>
        /// 参数化查询
        /// </summary>
        /// <param name="paramName">@p</param>
        /// <param name="paramValue">真实列</param>
        public QueryParam(string paramName, string paramValue)
        {
            if (!paramName.StartsWith("@"))
            {
                throw new ArgumentException(nameof(paramName));
            }

            ParamName = paramName;
            ParamValue = paramValue;
        }

        public override string OptionName => ParamName;

        /// <summary>
        /// 参数名
        /// </summary>
        public string ParamName { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; }

        public override string Builder()
        {
            return $"{OptionName}={ParamValue}";
        }

        public override string ToString()
        {
            return Builder();
        }
    }
}
