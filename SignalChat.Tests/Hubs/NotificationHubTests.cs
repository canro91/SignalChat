using Foundatio.Queues;
using ServiceStack.OrmLite;
using SignalChat.Bot.Contracts;
using SignalChat.Bot.Services;
using SignalChat.Controllers;
using SignalChat.Core.Contracts;
using SignalChat.Core.Domain;
using SignalChat.Core.Tasks;
using SignalChat.Database.Migrations;
using SignalChat.Database.Repositories;
using SignalChat.Hubs;
using SignalR_UnitTestingSupportMSTest.Hubs;
using System.Text;

namespace SignalChat.Tests.Hubs
{
    [TestClass]
    public class NotificationHubTests : HubUnitTestsBase
    {
        [TestMethod]
        public async Task Send_AnyMessage_SavesMessage()
        {
            var messageRepository = BuildRepository();
            var senderName = "username";
            var hub = BuildHub(messageRepository, senderName);

            var message = "This is a message";
            await hub.Send(message);

            var controller = new MessageController(messageRepository);
            var messages = await controller.GetAsync();

            Assert.IsNotNull(messages);
            Assert.AreEqual(1, messages.Count());

            var first = messages.First();
            Assert.AreEqual(senderName, first.Username);
            Assert.AreEqual(message, first.Body);
        }

        [TestMethod]
        public async Task Send_StockCommand_DoesNotSaveMessage()
        {
            var ticker = "msft";
            var price = 480f;

            var messageRepository = BuildRepository();
            var stockService = new FakeStockService(ticker, price);
            var queue = new InMemoryQueue<Message>();
            var hub = BuildHub(messageRepository, stockService: stockService, queue: queue);

            var command = $"/stock={ticker}";
            await hub.Send(command);

            var controller = new MessageController(messageRepository);
            var messages = await controller.GetAsync();

            Assert.IsNotNull(messages);
            Assert.IsFalse(messages.Any());

            var queueItem = await queue.DequeueAsync();
            Assert.IsNotNull(queueItem);
            Assert.IsNotNull(queueItem.Value);

            var message = queueItem.Value;
            StringAssert.Contains(message.Body, ticker, StringComparison.OrdinalIgnoreCase);
            StringAssert.Contains(message.Body, price.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        private NotificationHub BuildHub(IMessageRepository messageRepository,
                                         string? username = null,
                                         FakeStockService? stockService = null,
                                         InMemoryQueue<Message>? queue = null)
        {
            stockService ??= new FakeStockService("VOO", 480);
            queue ??= new InMemoryQueue<Message>();
            var botService = new BotService(stockService, queue);

            var messageService = new SendMessageService(messageRepository, botService);
            var hub = new NotificationHub(messageService);
            AssignToHubRequiredProperties(hub);

            ContextMock
                .Setup(x => x.UserIdentifier)
                .Returns(username ?? "anything");

            return hub;
        }

        private static MessageRepository BuildRepository()
        {
            var factory = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            var migrator = new Migrator(factory, typeof(Migration001).Assembly);
            migrator.Run();

            return new MessageRepository(factory);
        }
    }

    public class FakeStockService : IStockService
    {
        public string Ticker;
        public float ClosePrice;

        public FakeStockService(string ticker, float closePrice)
        {
            Ticker = ticker;
            ClosePrice = closePrice;
        }

        public Task<byte[]> FindStockQuoteAsync(string stockCode)
        {
            var stock = @$"Symbol,Date,Time,Open,High,Low,Close,Volume
{Ticker},2019-04-26,22:00:20,204.9,205,202.12,{ClosePrice},18649102";

            return Task.FromResult(Encoding.UTF8.GetBytes(stock));
        }
    }
}