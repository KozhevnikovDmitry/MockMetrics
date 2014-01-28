using System.Net.Sockets;
using Common.DA.ProviderConfiguration;

namespace Common.BL
{
    public class ConnectChecker
    {
        private readonly IProviderConfiguration _configuration;

        public ConnectChecker(IProviderConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Check()
        {
            try
            {
                using (var client = new TcpClient())
                {
                    client.Connect(_configuration.Server, _configuration.Port);
                }
                return true;
            }
            catch (SocketException)
            {
                return false;
            }
        }
    }
}
