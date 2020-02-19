using Api2.Interfaces;
using Api2.Requests;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MvcPkceClient.Web.Handlers
{
    public class Api3RequestHandler : IRequestHandler<Api3Request, string>
    {
        private IApi3ServiceClient api3ServiceClient;

        public Api3RequestHandler(IApi3ServiceClient api3ServiceClient)
        {
            this.api3ServiceClient = api3ServiceClient;
        }

        public async Task<string> Handle(Api3Request request, CancellationToken cancellationToken)
        {
            var result = await api3ServiceClient.Get();
            return result;
        }
    }
}
