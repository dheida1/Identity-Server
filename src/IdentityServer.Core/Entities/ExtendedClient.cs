using IdentityServer4.Models;
using System;

namespace IdentityServer.Core.Entities
{
    public class ExtClient : Client
    {
        public ExtendedClient ExtendedClient { get; set; }
    }

    public class ExtendedClient
    {
        public Guid Id { get; set; }
        public ClientType ClientType { get; set; }
        public string RawCertData { get; set; }
        public bool RequireJwe { get; set; } = true;
        public virtual ExtClient Client { get; set; }
        //[ForeignKey("ExtClient")]
        public int ClientId { get; set; }
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
