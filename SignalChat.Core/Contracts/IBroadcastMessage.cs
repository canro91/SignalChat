namespace SignalChat.Core.Contracts
{
    public interface IBroadcastMessage
    {
        void BroadcastMessage(string message, string fromUsername);
    }
}
