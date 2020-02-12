using System.Threading.Tasks;

namespace MvcPkceClient.Web.Interfaces
{
    public interface IApi1ServiceClient
    {
        Task<string> Get();
    }
}
