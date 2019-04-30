using Foundatio.Jobs;
using SignalChat.Factories;
using SignalChat.Jobs;
using SignalChat.Services;
using System.Web;
using System.Web.Http;

namespace SignalChat
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var messageQueue = SignalChatFactory.CreateQueue();
            var broadcastMessage = new SignalRBroadcastMessage();
            var job = new SendMessageIntoChatRoom(messageQueue, broadcastMessage);

            new JobRunner(job, runContinuous: true).RunAsync();
        }
    }
}
