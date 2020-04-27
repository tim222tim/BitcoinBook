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
            var socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(new IPEndPoint(ipAddress, testnet ? 18333 : 8333));

            try
            {
                var message = new VersionMessage(VersionMessage.DefaultVersion, 0, DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    0, new IPAddress(new byte[] {0, 0, 0, 0}), 18333,
                    0, new IPAddress(new byte[] {0, 0, 0, 0}), 18333,
                    0xa127ec40a4d7a8f6, "/rossitertest:0.1/", 0, true);
                var envelope = new NetworkEnvelope(message, testnet);

                var envelopeBytes = envelope.ToBytes();
                var bytesSent = socket.Send(envelopeBytes);

                // var bytes = new byte[16384];
                // var bytesReceived = socket.Receive(bytes);

                var responseEnvelope = NetworkEnvelope.Parse(new NetworkStream(socket), testnet);
                Assert.Equal("version", responseEnvelope.Command);
                var responseMessage = VersionMessage.Parse(responseEnvelope.Payload);
            }
            finally
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
    }
}
