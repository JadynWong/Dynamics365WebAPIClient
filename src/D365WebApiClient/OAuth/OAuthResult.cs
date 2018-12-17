namespace D365WebApiClient.OAuth
{
    public class OAuthResult
    {
        #region Filed

        ///<summary>
        /// Token
        /// </summary>
        public string access_token { get; set; }

        ///<summary>
        /// 类型
        /// </summary>
        public string token_type { get; set; }

        ///<summary>
        /// 有效期 秒
        /// </summary>
        public int expires_in { get; set; }

        ///<summary>
        /// refresh_Token 用于刷新Token
        /// </summary>
        public string refresh_token { get; set; }

        #endregion Filed
    }
}