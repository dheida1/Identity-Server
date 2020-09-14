using MediatR;
using MvcMtlsClient.Web.Interfaces;
using MvcMtlsClient.Web.Requests;
using System.Threading;
using System.Threading.Tasks;

namespace MvcMtlsClient.Web.Handlers
{
    public class Api1RequestHandler : IRequestHandler<Api1Request, string>
    {
        private IApi1UserService api1Service;

        public Api1RequestHandler(IApi1UserService api1Service)
        {
            this.api1Service = api1Service;
        }

        public async Task<string> Handle(Api1Request request, CancellationToken cancellationToken)
        {
            var result = await api1Service.Get();
            return result;
        }
    }
}
