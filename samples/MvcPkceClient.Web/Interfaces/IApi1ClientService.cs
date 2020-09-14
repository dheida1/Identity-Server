using System.Threading.Tasks;

namespace MvcPkceClient.Web.Interfaces
{
    public interface IApi1ClientService
    {
        Task<string> Get();
    }
}
