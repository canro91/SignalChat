using Foundatio.Queues;
using SignalChat.Core.Domain;

namespace SignalChat.Factories
{
    public class SignalChatFactory
    {
        private static readonly Lazy<IQueue<Message>> MessageQueue = new Lazy<IQueue<Message>>(() => new InMemoryQueue<Message>());

        public static Lazy<IQueue<Message>> CreateQueue()
        {
            return MessageQueue;
        }
    }
}