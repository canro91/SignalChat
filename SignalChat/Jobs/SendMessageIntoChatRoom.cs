using Foundatio.Jobs;
using Foundatio.Queues;
using SignalChat.Core.Contracts;
using SignalChat.Core.Domain;

namespace SignalChat.Jobs
{
    public class SendMessageIntoChatRoom : QueueJobBase<Message>
    {
        private readonly IBroadcastMessage _broadcastMessage;

        public SendMessageIntoChatRoom(Lazy<IQueue<Message>> queue,
                                       IBroadcastMessage broadcastMessage)
            : base(queue)
        {
            _broadcastMessage = broadcastMessage;
        }

        protected override Task<JobResult> ProcessQueueEntryAsync(QueueEntryContext<Message> context)
        {
            var message = context.QueueEntry.Value;
            _broadcastMessage.BroadcastMessage(message.Body, message.Username);

            return Task.FromResult(JobResult.Success);
        }
    }
}
