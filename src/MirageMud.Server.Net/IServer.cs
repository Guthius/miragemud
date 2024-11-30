namespace MirageMud.Server.Net;

public interface IServer<out TClient, TClientState>
    where TClient : Connection<TClient, TClientState>
    where TClientState : Enum
{
    void SendTo(IPacket packet, Func<TClient, bool> predicate);
}