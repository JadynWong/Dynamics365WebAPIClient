using System;
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

namespace D365WebApiClient.Options
{
    public class Dynamics365Option
    {
        /// <summary>
        /// ADFS地址
        /// </summary>
        public string ADFSUri { get; set; }

        /// <summary>
        /// CRM
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// Organization
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// ClientId
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// ClientSecret Dynamics365Type = IfdV4 | Online
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// RedirectUri ClientSecret Dynamics365Type = IfdV3
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
        /// Dynamics365Type
        /// </summary>
        public Dynamics365Type Dynamics365Type { get; set; }

        /// <summary>
        /// API版本
        /// </summary>
        public Version Version { get; set; }

        private const string ApiPath = "api/data/";

        public Dynamics365Option()
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
        /// <param name="dynamics365Type"></param>
        public Dynamics365Option(string adfsUri, string resource, string domainName, string userName, string password,
            string clientId, string redirectUri, string organization, string version, Dynamics365Type dynamics365Type)
        {
            ADFSUri = adfsUri;
            Resource = resource;
            DomainName = domainName;
            UserName = userName;
            Password = password;
            ClientId = clientId;
            RedirectUri = redirectUri;
            Organization = organization;
            Dynamics365Type = dynamics365Type;
            Version = new Version(version);
        }

        public string WebApiAddress
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Resource) || Version == null)
                {
                    throw new Exception("Dynamics365Options must be configuare");
                }

                switch (Dynamics365Type)
                {
                    case Dynamics365Type.IFD_ADFS_V4:
                        if (!this.Resource.EndsWith("/"))
                        {
                            return this.Resource + "/";
                        }
                        return this.Resource;
                    case Dynamics365Type.Online:
                        //todo online resource
                        throw new ArgumentOutOfRangeException();
                    case Dynamics365Type.IFD_ADFS_V3:
                        if (Organization == null)
                        {
                            throw new Exception("Dynamics365Options OnPromise Organization must be configuare");
                        }
                        return $"{Resource}{Organization}{ApiPath}v{Version}/";

                    case Dynamics365Type.OnPremises:
                        return $"{Resource}{ApiPath}v{Version}/";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    public enum Dynamics365Type
    {
        /// <summary>
        /// Online
        /// </summary>
        Online,
        /// <summary>
        /// IFD_ADFS_V3 (Must set RedirectUri)
        /// </summary>
        IFD_ADFS_V3,
        /// <summary>
        /// ADFS4.0 (Must set ClientSecret)
        /// </summary>
        IFD_ADFS_V4,
        /// <summary>
        /// On-Promise
        /// </summary>
        OnPremises
    }
}