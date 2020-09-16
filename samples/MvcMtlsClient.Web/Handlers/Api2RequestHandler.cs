using MediatR;
using MvcMtlsClient.Web.Interfaces;
using MvcMtlsClient.Web.Requests;
using System.Threading;
using System.Threading.Tasks;

namespace MvcMtlsClient.Web.Handlers
{
    public class Api2RequestHandler : IRequestHandler<Api2Request, string>
    {
        private IApi2UserService api2UserService;

        public Api2RequestHandler(IApi2UserService api2UserService)
        {
            this.api2UserService = api2UserService;
        }

        public async Task<string> Handle(Api2Request request, CancellationToken cancellationToken)
        {
            var result = await api2UserService.Get();
            return result;
        }
    }
}
