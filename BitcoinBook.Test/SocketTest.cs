using System;
using System.Net;
using System.Net.Sockets;
using Xunit;

namespace BitcoinBook.Test
{
    public class SocketTest
    {
        [Fact]
        public void SendMessageProgrammingBitcoinTestnet()
        {
            var hostEntry = Dns.GetHostEntry("testnet.programmingbitcoin.com");

            SendVersion(hostEntry.AddressList[0], true);
        }

        [Fact]
        public void SendMessageTimsIsland()
        {
            SendVersion(new IPAddress(new byte[] { 104, 62, 47, 181 }), false);
        }

        void SendVersion(IPAddress ipAddress, bool testnet)
        {
            var gotVerAck = false;
            string agent = null;

            var socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            var port = (ushort) (testnet ? 18333 : 8333);
            socket.Connect(new IPEndPoint(ipAddress, port));

            try
            {
                var stream = new NetworkStream(socket);
                new NetworkEnvelope(new VersionMessage(), testnet).WriteTo(stream);

                while (agent == null && !gotVerAck)
                {
                    var responseEnvelope = NetworkEnvelope.Parse(stream, testnet);
                    agent = (responseEnvelope.Message as VersionMessage)?.UserAgent;
                    gotVerAck = responseEnvelope.Message is VerAckMessage;
                }

                Assert.StartsWith("/Satoshi", agent ?? "");
            }
            finally
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
    }
}
