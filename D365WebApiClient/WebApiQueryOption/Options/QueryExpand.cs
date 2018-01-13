using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365WebApiClient.WebApiQueryOption.Options
{
    /// <summary>
    /// 通过扩展导航属性检索相关实体
    /// <para>https://msdn.microsoft.com/zh-cn/library/gg334767.aspx#通过扩展导航属性检索相关实体</para>
    /// <para>https://msdn.microsoft.com/zh-cn/library/mt607871.aspx#通过扩展导航属性检索实体的相关实体</para>
    /// </summary>
    public class QueryExpand : QueryOption
    {
        private static Type[] SupportTypes;

        static QueryExpand()
        {
            SupportTypes = new Type[] { typeof(QuerySelect), typeof(QueryFilter), typeof(QueryOrderBy), typeof(QueryTop) };

        }

        public QueryExpand()
        {
            ExpandNavigationProperties = new Dictionary<string, IList<QueryOption>>();
        }

        public QueryExpand(string navigationPropertiy)
        {
            ExpandNavigationProperties =
                new Dictionary<string, IList<QueryOption>>
                {
                    { navigationPropertiy, null }
                };
        }

        /// <summary>
        /// 通过扩展导航属性检索相关实体
        /// </summary>
        /// <param name="navigationPropertiy">单一值 和 集合值 导航属性</param>
        /// <param name="queryOptions">
        /// 展开的实体的选项
        /// <para>单一值Lookup 只支持$select</para>
        /// <para>集合值 支持 $select、$filter、$orderby 和 $top</para>
        /// <para>https://msdn.microsoft.com/zh-cn/library/mt607871.aspx#应用于展开的实体的选项</para>
        /// </param>
        public QueryExpand(string navigationPropertiy, IList<QueryOption> queryOptions)
        {
            ExpandNavigationProperties =
                new Dictionary<string, IList<QueryOption>>
                {
                    { navigationPropertiy, queryOptions }
                };
        }

        /// <summary>
        /// 导航属性集合
        /// </summary>
        /// <param name="expandNavigationProperties">
        /// <para>单一值Lookup 只支持$select</para>
        /// <para>集合值 支持 $select、$filter、$orderby 和 $top</para>
        /// <para>https://msdn.microsoft.com/zh-cn/library/mt607871.aspx#应用于展开的实体的选项</para>
        /// </param>
        public QueryExpand(Dictionary<string, IList<QueryOption>> expandNavigationProperties)
        {
            ExpandNavigationProperties = expandNavigationProperties;
        }

        /// <summary>
        /// 返回相关实体的引用（链接）
        /// </summary>
        /// <param name="navigationPropertiy">单一值 导航属性 lookup</param>
        /// <param name="returnRef">返回相关引用连接</param>
        public QueryExpand(string navigationPropertiy, bool returnRef)
        {
            ExpandNavigationProperties = new Dictionary<string, IList<QueryOption>>();
            ExpandNavigationProperties.Add(navigationPropertiy, null);
            ReturnRef = returnRef;
        }

        /// <summary>
        /// 导航属性集合
        /// <para>单一值Lookup 只支持$select</para>
        /// <para>集合值 支持 $select、$filter、$orderby 和 $top</para>
        /// <para>https://msdn.microsoft.com/zh-cn/library/mt607871.aspx#应用于展开的实体的选项</para>
        /// </summary>
        public Dictionary<string, IList<QueryOption>> ExpandNavigationProperties { get; set; }

        /// <summary>
        /// 单值导航属性lookup
        /// </summary>
        public bool ReturnRef { get; set; }

        public override string OptionName => "$expand";

        public override string Builder()
        {
            if (ReturnRef)
            {
                if (ExpandNavigationProperties.Count != 1)
                {
                    throw new ArgumentException("ReturnRef=true时，只能有一项", nameof(ExpandNavigationProperties));
                }

                var navigationPropertiy = ExpandNavigationProperties.First();
                if (string.IsNullOrWhiteSpace(navigationPropertiy.Key))
                {
                    throw new ArgumentException("navigationPropertiy 不能未空白", navigationPropertiy.Key);
                }
                return $"{OptionName}={navigationPropertiy.Key}/$ref";
            }

            if (ExpandNavigationProperties.Count < 1)
            {
                return string.Empty;
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(OptionName);
            stringBuilder.Append("=");
            var length = stringBuilder.Length;
            foreach (var navigationPropertiy in ExpandNavigationProperties)
            {
                if (stringBuilder.Length > length)
                {
                    stringBuilder.Append(",");
                }

                if (string.IsNullOrWhiteSpace(navigationPropertiy.Key))
                {
                    throw new ArgumentException("navigationPropertiy 不能未空白", navigationPropertiy.Key);
                }

                stringBuilder.Append(navigationPropertiy.Key);

                if (navigationPropertiy.Value == null)
                {
                    stringBuilder.Append(navigationPropertiy.Key);
                    continue;
                }
                var optionBuilder = new StringBuilder();
                optionBuilder.Append("(");
                var queryOptionNames = new List<string>();
                foreach (var queryOption in navigationPropertiy.Value)
                {
                    if (queryOption == null)
                    {
                        continue;
                    }

                    if (queryOptionNames.Contains(queryOption.OptionName))
                    {
                        throw new Exception($"参数'{queryOption.OptionName}'出现多次,参数仅能出现一次");
                    }

                    var currentType = queryOption.GetType();
                    if (!SupportTypes.Contains(currentType))
                    {
                        throw new ArgumentException("不支持的查询类型", currentType.FullName);
                    }
                    queryOptionNames.Add(queryOption.OptionName);
                    if (queryOptionNames.Contains(QueryCount.Name) && queryOptionNames.Contains(QueryTop.Name))
                    {
                        throw new Exception($"您不应将 $top 与 $count 一起使用.https://msdn.microsoft.com/zh-cn/library/gg334767.aspx#%E9%99%90%E5%88%B6%E7%BB%93%E6%9E%9C");
                    }
                    var queryOptionStr = queryOption.ToString();
                    if (string.IsNullOrWhiteSpace(queryOptionStr))
                        continue;
                    if (optionBuilder.Length > 1)
                    {
                        optionBuilder.Append(";");
                    }
                    optionBuilder.Append(queryOptionStr);
                }

                if (optionBuilder.Length > 1)
                {
                    optionBuilder.Append(")");
                    stringBuilder.Append(optionBuilder.ToString());
                }
               

            }

            if (stringBuilder.Length == length)
            {
                return string.Empty;
            }

            return stringBuilder.ToString();

        }
    }
}
