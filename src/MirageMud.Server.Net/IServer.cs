namespace MirageMud.Server.Net;

public interface IServer<out TConnection> where TConnection : Connection<TConnection>
{
    void SendTo(IPacket packet, Func<TConnection, bool> predicate);
}