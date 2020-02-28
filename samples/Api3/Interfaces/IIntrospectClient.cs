using System.Threading.Tasks;

namespace Api3.Interfaces
{
    public interface IIntrospectClient
    {
        Task<string> Get();
    }
}
