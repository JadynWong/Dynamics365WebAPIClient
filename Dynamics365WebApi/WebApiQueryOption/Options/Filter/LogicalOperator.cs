using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics365WebApi.WebApiQueryOption.Options.Filter
{
    /// <summary>
    /// 逻辑运算符
    /// </summary>
    public enum LogicalOperator
    {
        /// <summary>
        /// and 逻辑与
        /// </summary>
        LogicalAnd,
        /// <summary>
        /// or 逻辑或
        /// </summary>
        LogicalOr,
        /// <summary>
        /// not 逻辑非
        /// </summary>
        LogicalNegation,
    }
}
