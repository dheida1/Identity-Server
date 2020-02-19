using System.Threading.Tasks;

namespace Api3.Interfaces
{
    public interface IApi3ServiceClient
    {
        Task<string> Get();
    }
}
