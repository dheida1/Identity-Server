using System.Threading.Tasks;

namespace MvcPkceClient.Web.Interfaces
{
    public interface IApi2ServiceClient
    {
        Task<string> Get();
        Task<string> Delegate();
    }
}
