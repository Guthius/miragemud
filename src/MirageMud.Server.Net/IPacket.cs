namespace MirageMud.Server.Net;

public interface IPacket
{
    void WriteTo(IPacketWriter writer);
}

public interface IPacket<out TSelf> : IPacket where TSelf : IPacket<TSelf>?
{
    static abstract TSelf ReadFrom(IPacketReader reader);
}