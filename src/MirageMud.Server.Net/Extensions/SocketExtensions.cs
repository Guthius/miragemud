using System.Net;
using System.Net.Sockets;

namespace MirageMud.Server.Net.Extensions;

internal static class SocketExtensions
{
    public static string GetRemoteIp(this Socket socket)
    {
        return ((IPEndPoint) socket.RemoteEndPoint!).Address.ToString();
    }
}