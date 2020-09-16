namespace IdentityServer.Infrastructure.Entities
{
    public class ClientType
    {
        //public const string Empty = 0,
        public const string WebHybrid = "WebHybrid"; // A server-side application running on your infrastructure.
        public const string Spa = "Spa"; //A client-side application running in a browser.
        public const string Native = "Native"; //A desktop or mobile application running on a user's device.
        public const string Machine = "Machine"; // A machine-to-machine method of communication.
        public const string Device = "Device"; //An IoT application or otherwise browserless or input constrained device.
        public const string Confidential = "confidential"; //confidential clients
    }
}
