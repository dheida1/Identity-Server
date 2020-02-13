using System.Threading.Tasks;

namespace MvcJwtClient.Web.Interfaces
{
    public interface IApi1ServiceClient
    {
        Task<string> Get();
    }
}
