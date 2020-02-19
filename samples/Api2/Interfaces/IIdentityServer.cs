using System.Threading.Tasks;

namespace Api2.Interfaces
{

    public interface IIdentityServerClient
    {
        Task<string> RequestTokenAsync();
    }
}

