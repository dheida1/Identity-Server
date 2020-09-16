using System.Threading.Tasks;

namespace MvcPkceClient.Web.Interfaces
{
    public interface IApi2ClientService
    {
        Task<string> Get();
        Task<string> Delegate();
    }
}
