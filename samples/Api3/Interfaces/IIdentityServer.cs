using System.Threading.Tasks;

namespace Api3.Interfaces
{

    public interface IIdentityServerClient
    {
        Task<string> RequestTokenAsync();
    }
}

