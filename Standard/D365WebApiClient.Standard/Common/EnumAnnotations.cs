namespace D365WebApiClient.Standard.Common
{
    //[Flags]
    public enum EnumAnnotations
    {
        /// <summary>
        /// 无
        /// <para></para>
        /// </summary>
        None = 0,
        /// <summary>
        /// 检索有关查找属性的数据
        /// <para>OData.Community.Display.V1.FormattedValue</para>
        /// </summary>
        FormattedValue = 1,
        /// <summary>
        /// 包括对实体的引用的单值导航属性的名称
        /// <para>Microsoft.Dynamics.CRM.associatednavigationproperty</para>
        /// </summary>
        Associatednavigationproperty = 2,
        /// <summary>
        /// 查找所引用的实体的逻辑名称
        /// <para>Microsoft.Dynamics.CRM.lookuplogicalname</para>
        /// </summary>
        Lookuplogicalname = 4,
        /// <summary>
        /// 匹配CRM
        /// <para>Microsoft.Dynamics.CRM.*</para>
        /// </summary>
        MicrosoftDynamicsCrmAll = 8,
        /// <summary>
        /// 包含所有
        /// <para>*</para>
        /// </summary>
        All = 16,
    }
}
