using System.Threading.Tasks;

namespace MvcClient.Web.Interfaces
{
    public interface IApi1ServiceClient
    {
        Task<string> Get();
    }
}
