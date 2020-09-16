using System.Threading.Tasks;

namespace MvcPkceClient.Web.Interfaces
{
    public interface IApi2UserService
    {
        Task<string> Get();
        Task<string> Delegate();
    }
}
