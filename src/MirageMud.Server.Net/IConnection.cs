namespace MirageMud.Server.Net;

public interface IConnection
{
    void Send(IPacket packet);
    void Disconnect();
}