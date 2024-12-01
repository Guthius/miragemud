namespace MirageMud.Server.Protocol;

public static class PacketId
{
    public static class FromServer
    {
        public const int Alert = 1;
        public const int CharacterList = 2;
        public const int CharacterTypes = 4;
    }

    public static class FromClient
    {
        public const int GetCharacterTypes = 1;
        public const int CreateAccount = 2;
        public const int Login = 4;
        public const int CreateCharacter = 5;
        public const int DeleteCharacter = 6;
        public const int SelectCharacter = 7;
    }
}