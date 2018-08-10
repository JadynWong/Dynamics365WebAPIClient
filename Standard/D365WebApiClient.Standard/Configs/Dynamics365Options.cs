using System;

namespace D365WebApiClient.Standard.Configs
{
    public class Dynamics365Options
    {
        /// <summary>
        /// ADFS地址
        /// </summary>
        public string ADFS_URI { get; set; }

        /// <summary>
        /// CRM
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// 组织
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// Clientguid
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// RedirectUri
        /// </summary>
        public string RedirectUri { get; set; }

        /// <summary>
        /// DomainName
        /// </summary>
        public string DomainName { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// IsIfd
        /// </summary>
        public bool IsIfd { get; set; }

        /// <summary>
        /// API版本
        /// </summary>
        public Version Version { get; set; }

        private const string ApiPath = "api/data/";

        public Dynamics365Options()
        {
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="adfsUri"></param>
        /// <param name="resource"></param>
        /// <param name="domainName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="clientId"></param>
        /// <param name="redirectUri"></param>
        /// <param name="organization"></param>
        /// <param name="version"></param>
        /// <param name="isIfd"></param>
        public Dynamics365Options(string adfsUri, string resource, string domainName, string userName, string password,
            string clientId, string redirectUri, string organization, string version, bool isIfd)
        {
            ADFS_URI = adfsUri;
            Resource = resource;
            DomainName = domainName;
            UserName = userName;
            Password = password;
            ClientId = clientId;
            RedirectUri = redirectUri;
            Organization = organization;
            IsIfd = isIfd;
            Version = new Version(version);
        }

        private string _url;

        public string WebApiAddress
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_url))
                {
                    return _url;
                }

                if (!this.Resource.EndsWith("/"))
                {
                    this.Resource += "/";
                }
                if (IsIfd)
                {
                    _url = $"{this.Resource}{ApiPath}v{Version}/";
                    return _url;
                }
                else
                {
                    _url = $"{this.Resource}{this.Organization}{ApiPath}v{Version}/";
                    return _url;
                }
            }
        }
    }
}