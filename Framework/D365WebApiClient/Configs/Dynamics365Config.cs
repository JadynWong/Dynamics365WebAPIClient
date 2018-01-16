using System;
using System.Configuration;

namespace D365WebApiClient.Configs
{
    public class Dynamics365Config
    {
        /// <summary>
        /// ADFS地址
        /// </summary>
        public string ADFS_URI { get; private set; }

        /// <summary>
        /// CRM
        /// </summary>
        public string Resource { get; private set; }

        /// <summary>
        /// 组织
        /// </summary>
        public string Organization { get; private set; }

        /// <summary>
        /// Clientguid
        /// </summary>
        public string ClientId { get; private set; }

        /// <summary>
        /// RedirectUri
        /// </summary>
        public string RedirectUri { get; private set; }

        /// <summary>
        /// DomainName
        /// </summary>
        public string DomainName { get; private set; }

        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// IsIfd
        /// </summary>
        public bool IsIfd { get; private set; }

        /// <summary>
        /// API版本
        /// </summary>
        public Version Version { get; private set; }

        private const string ApiPath = "/api/data/";


        public Dynamics365Config()
        {
            ADFS_URI = ConfigurationManager.AppSettings["ADFSUri"];
            Resource = ConfigurationManager.AppSettings["Resource"];
            DomainName = ConfigurationManager.AppSettings["DomainName"];
            UserName = ConfigurationManager.AppSettings["UserName"];
            Password = ConfigurationManager.AppSettings["Password"];
            ClientId = ConfigurationManager.AppSettings["ClientId"];
            RedirectUri = ConfigurationManager.AppSettings["RedirectUri"];
            Organization = ConfigurationManager.AppSettings["Organization"];
            string version = ConfigurationManager.AppSettings["Version"];
            var isIfdStr = ConfigurationManager.AppSettings["IsIfd"];
            IsIfd = string.Equals(isIfdStr, "true", StringComparison.CurrentCultureIgnoreCase);
            Version = new Version(version);
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
        public Dynamics365Config(string adfsUri, string resource, string domainName, string userName, string password,
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
                if (IsIfd)
                {
                    if (!string.IsNullOrWhiteSpace(_url))
                    {
                        return _url;
                    }

                    _url = $"{this.Resource}{ApiPath}v{Version}/";
                    return _url;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(_url))
                    {
                        return _url;
                    }

                    _url = $"{this.Resource}{this.Organization}{ApiPath}v{Version}/";
                    return _url;
                }
            }
        }
    }
}