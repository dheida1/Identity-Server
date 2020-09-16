//using MediatR;
//using MvcPkceClient.Web.Interfaces;
//using MvcPkceClient.Web.Requests;
//using System.Threading;
//using System.Threading.Tasks;

//namespace MvcPkceClient.Web.Handlers
//{
//    public class Api2DelegateRequestHandler : IRequestHandler<Api2DelegateRequest, string>
//    {
//        private IApi2UserService api2UserService;

//        public Api2DelegateRequestHandler(IApi2UserService api2UserService)
//        {
//            this.api2UserService = api2UserService;
//        }

//        public async Task<string> Handle(Api2DelegateRequest request, CancellationToken cancellationToken)
//        {
//            var result = await api2UserService.Delegate();
//            return result;
//        }
//    }
//}
