﻿using MediatR;
using MvcJwtClient.Web.Interfaces;
using MvcJwtClient.Web.Requests;
using System.Threading;
using System.Threading.Tasks;

namespace MvcJwtClient.Web.Handlers
{
    public class Api1RequestHandler : IRequestHandler<Api1Request, string>
    {
        private IApi1ServiceClient api1ServiceClient;

        public Api1RequestHandler(IApi1ServiceClient api1ServiceClient)
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
