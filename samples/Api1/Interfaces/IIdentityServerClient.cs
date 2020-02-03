using System.Threading.Tasks;

namespace Api1.Interfaces
{
    public class IIdentityServerClient
    {
        Task<string> RequestClientCredentialsTokenAsync()
    }
}
