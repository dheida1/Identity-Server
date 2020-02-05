using System.Threading.Tasks;

namespace MvcClient.Web.Interfaces
{
    public interface IApi2ServiceClient
    {
        Task<string> Get();
    }
}
