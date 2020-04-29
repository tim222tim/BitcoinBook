using System;
using System.Net;
using System.Net.Sockets;

namespace BitcoinBook
{
    public class SimpleNode : IDisposable
    {
        readonly bool testnet;

        readonly Socket socket;
        readonly NetworkStream stream;

        public string RemoteUserAgent { get; private set; }

        public SimpleNode(IPAddress remoteHost, bool testnet = false)
        {
            this.testnet = testnet;
            socket = new Socket(remoteHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(new IPEndPoint(remoteHost, (ushort)(testnet ? 18333 : 8333)));
            stream = new NetworkStream(socket);
        }

        public void Handshake()
        {
            Send(new VersionMessage());

            var gotVerAck = false;
            string agent = null;

            while (agent == null && !gotVerAck)
            {
                var responseEnvelope = NetworkEnvelope.Parse(stream, testnet);
                agent = (responseEnvelope.Message as VersionMessage)?.UserAgent;
                gotVerAck = responseEnvelope.Message is VerAckMessage;
            }

            RemoteUserAgent = agent;
        }

        public void Send(VersionMessage message)
        {
            new NetworkEnvelope(message, testnet).WriteTo(stream);
        }

        public void Dispose()
        {
            stream?.Dispose();
            socket?.Dispose();
        }
    }
}
