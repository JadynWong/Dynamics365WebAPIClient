using System.Threading.Tasks;

namespace D365WebApiClient.OAuth
{
    public interface IOAuthService
    {
        Task<OAuthResult> AcquireTokenAsync();
        Task<OAuthResult> CrmRefreshTokenAsync(string refresh_token);
    }
}
