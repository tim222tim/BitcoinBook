﻿using System;
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
            stream = new NetworkStream(socket) {ReadTimeout = 4000};
        }

        public void Handshake()
        {
            RemoteUserAgent = null;
            Send(new VersionMessage());
            WaitFor<VerAckMessage>();
            if (RemoteUserAgent == null)
            {
                WaitFor<VersionMessage>();
            }
        }

        public IMessage WaitForMessage()
        {
            var envelope = NetworkEnvelope.Parse(stream, testnet);
            if (envelope.Message is VersionMessage versionMessage)
            {
                RemoteUserAgent = versionMessage.UserAgent;
                Send(new VerAckMessage());
            }
            // TODO ping message
            return envelope.Message;
        }

        public T WaitFor<T>() where T : class, IMessage
        {
            while (true)
            {
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
