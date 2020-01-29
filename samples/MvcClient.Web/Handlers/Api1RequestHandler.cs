using MediatR;
using MvcClient.Web.Requests;
using System.Threading;
using System.Threading.Tasks;

namespace MvcClient.Web.Handlers
{
    public class Api1RequestHandler : IRequestHandler<Api1Request, string>
    {
        private IApi1Service api1Service;

        public Api1RequestHandler(IApi1Service api1Service)
        {
            this.api1Service = api1Service;
        }

        public Task<string> Handle(Api1Request request, CancellationToken cancellationToken)
        {

            return Task.FromResult();
        }
    }
}
