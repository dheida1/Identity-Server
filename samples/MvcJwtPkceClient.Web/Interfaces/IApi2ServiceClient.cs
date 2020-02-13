using System.Threading.Tasks;

namespace MvcJwtClient.Web.Interfaces
{
    public interface IApi2ServiceClient
    {
        Task<string> Get();
    }
}
