using MediatR;
using MvcPkceClient.Web.Interfaces;
using MvcPkceClient.Web.Requests;
using System.Threading;
using System.Threading.Tasks;

namespace MvcPkceClient.Web.Handlers
{
    public class Api2RequestHandler : IRequestHandler<Api2Request, string>
    {
        private IApi2UserService api2ServiceClient;

        public Api2RequestHandler(IApi2UserService api2ServiceClient)
        {
            this.api2ServiceClient = api2ServiceClient;
        }

        public async Task<string> Handle(Api2Request request, CancellationToken cancellationToken)
        {
            var result = await api2ServiceClient.Get();
            return result;
        }
    }
}
