using System;
using System.Collections;
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
            stream = new NetworkStream(socket) {ReadTimeout = 500};
        }

        public void Handshake()
        {
            Send(new VersionMessage());

            var gotVerAck = false;
            string agent = null;

            while (agent == null && !gotVerAck)
            {
                var message = WaitForMessage();
                agent = (message as VersionMessage)?.UserAgent;
                gotVerAck = message is VerAckMessage;
            }

            RemoteUserAgent = agent;
        }

        public IMessage WaitForMessage()
        {
            var responseEnvelope = NetworkEnvelope.Parse(stream, testnet);
            return responseEnvelope.Message;
        }

        public T WaitFor<T>() where T : class, IMessage
        {
            while (true)
            {
                // TODO respond to version & ping messages
                if (WaitForMessage() is T message)
                {
                    return message;
                }
            }
        }

        public void Send(IMessage message)
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
