using System.Threading.Tasks;

namespace MvcClient.Web.Interfaces
{
    public interface IIdentityServerClient
    {
        Task<string> RequestClientCredentialsTokenAsync();
        //Task<string> RequestRefreshTokenAsync();
    }
}
