using MediatR;
using MvcPkceClient.Web.Interfaces;
using MvcPkceClient.Web.Requests;
using System.Threading;
using System.Threading.Tasks;

namespace MvcPkceClient.Web.Handlers
{
    public class Api1RequestHandler : IRequestHandler<Api1Request, string>
    {
        private IApi1UserService api1ServiceClient;

        public Api1RequestHandler(IApi1UserService api1ServiceClient)
        {
            this.api1ServiceClient = api1ServiceClient;
        }

        public async Task<string> Handle(Api1Request request, CancellationToken cancellationToken)
        {
            var result = await api1ServiceClient.Get();
            return result;
        }
    }
}
