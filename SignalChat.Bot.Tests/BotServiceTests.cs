using Foundatio.Queues;
using SignalChat.Bot.Contracts;
using SignalChat.Core.Domain;
using System.Text;

namespace SignalChat.Bot.Tests
{
    [TestClass]
    public class BotServiceTests
    {
        [TestMethod]
        public async Task BotService_AGivenCommand_EnqueueMessage()
        {
            var stock = @"Symbol,Date,Time,Open,High,Low,Close,Volume
AAPL.US,2019-04-26,22:00:20,204.9,205,202.12,204.3,18649102";
            var fakeStockService = new FakeStockService(stock);
            var messageQueue = new InMemoryQueue<Message>();
            var botService = new BotService(fakeStockService, messageQueue);

            await botService.QueryAndSend(stockCode: "aapl.us");

            var lastMessage = await messageQueue.DequeueAsync();
            Assert.IsNotNull(lastMessage);
        }
    }

    public class FakeStockService : IStockService
    {
        public string Stock;

        public FakeStockService(string stock)
        {
            this.Stock = stock;
        }

        public byte[] FindStockQuote(string stockCode)
        {
            return Encoding.UTF8.GetBytes(Stock);
        }
    }
}