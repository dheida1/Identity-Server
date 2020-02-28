using IdentityServer4.EntityFramework.Entities;

namespace IdentityServer.Infrastructure.Dto
{
    public class ExtClient : Client
    {
        //public int? ExtendedClientId { get; set; }
        public ExtendedClient ExtendedClient { get; set; }
    }

    public class ExtendedClient
    {
        public int Id { get; set; }
        public ClientType ClientType { get; set; }
        public string RawCertData { get; set; }
        public bool RequireJwe { get; set; } = true;
    }


    public enum ClientType
    {
        Empty = 0,
        WebHybrid = 1, // A server-side application running on your infrastructure.
        Spa = 2, //A client-side application running in a browser.
        Native = 3, //A desktop or mobile application running on a user's device.
        Machine = 4, // A machine-to-machine method of communication.
        Device = 5 //An IoT application or otherwise browserless or input constrained device.
    }
}

