using MediatR;
using MvcPkceClient.Web.Interfaces;
using MvcPkceClient.Web.Requests;
using System.Threading;
using System.Threading.Tasks;

namespace MvcPkceClient.Web.Handlers
{
    public class Api2DelegateRequestHandler : IRequestHandler<Api2DelegateRequest, string>
    {
        private IApi2ServiceClient api2ServiceClient;

        public Api2DelegateRequestHandler(IApi2ServiceClient api2ServiceClient)
        {
            this.api2ServiceClient = api2ServiceClient;
        }

        public async Task<string> Handle(Api2DelegateRequest request, CancellationToken cancellationToken)
        {
            var result = await api2ServiceClient.Delegate();
            return result;
        }
    }
}
