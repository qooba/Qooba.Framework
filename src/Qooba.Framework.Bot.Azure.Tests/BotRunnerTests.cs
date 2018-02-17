using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Qooba.Framework.Bot.Abstractions;
using Xunit;
using Qooba.Framework.Bot.Attributes;
using Qooba.Framework.Serialization;
using Qooba.Framework.Logging.AzureApplicationInsights;
using Qooba.Framework.Bot.Abstractions.Form;
using Qooba.Framework.Bot.Abstractions.Models;

namespace Qooba.Framework.Bot.Azure.Tests
{
    public class BotRunnerTests : BaseRunnerTests
    {
        [Fact]
        public void RunHello()
        {
            Run("hello");
        }

        [Fact]
        public void RunHello1()
        {
            Run("rawattachment");
        }

        [Fact]
        public void RunShopping()
        {
            Run("Jad� do Arkadii chc� kupi� spodnie");
        }

        [Fact]
        public void RunReplyAction()
        {
            //Run("Show replyAction");
            Run("Moje Konto 360");
        }

        [Fact]
        public void RunText()
        {
            Run("Show text");
        }

        [Fact]
        public void RunImage()
        {
            Run("Show image");
        }

        [Fact]
        public void RunVideo()
        {
            Run("Show video");
        }

        [Fact]
        public void RunFile()
        {
            Run("Show file");
        }

        [Fact]
        public void RunEnum()
        {
            Run("Show enum");
        }

        [Fact]
        public void RunLocation()
        {
            Run("Show location");
        }

        [Fact]
        public void RunForm()
        {
            Run("Show form");
            Run("Red");
            Run("Rectangle");
        }

        [Fact]
        public void RunPostbackButtonTemplate()
        {
            Run("Show postback button template");
        }

        [Fact]
        public void RunPostbackCarousel()
        {
            Run("Show postback carousel");
        }

        [Fact]
        public void RunTestReplyAction()
        {
            Run("Show test reply action");
        }

        [Fact]
        public void RunTestMyModel()
        {
            Run("Show test model");
        }

        [Fact]
        public void RunTest1MyModel()
        {
            Run("Show test name MyValue");
            Task.Delay(2000);
            Run("MyName");
            Task.Delay(2000);
            Run("MySurnameName");
        }

        private void Run(string message)
        {
            var container = new Container();
            container.RegisterInstance(null, typeof(IConfigurationRoot), this.configurationRootMock.Object);

            FrameworkBuilder.Create()
            .AddModule(m => m.Module(new SerializationModule()))
            .AddModule(m => m.Module(new BotModule()))
            .AddModule(m => m.Module(new BotAzureModule()))
            .AddModule(m => m.Module(new AzureApplicationInsightsModule()))
            .AddBotAction<TestReplyAction>()
            .AddBotForm<MyModel>()
            .Bootstrapp();
            
            string queueItem = QueueItem(message);
            BotRunner.Run(queueItem).Wait();
        }

        private string QueueItem(string message) => ("{\"connectorType\": \"Messanger\", \"message\":{\"sender\": {\"id\": \"USER_ID\"},\"recipient\": {\"id\": \"PAGE_ID\"},\"timestamp\": 1458692752478,\"message\": {\"mid\": \"mid.1457764197618:41d102a3e1ae206a38\",\"text\": \"" + message + "\",\"quick_reply\": {}}}}").Replace("USER_ID", this.UserId);
    }

    [Route("Show test reply action")]
    public class TestReplyAction : IReplyAction<TextReplyMessage>
    {
        public async Task<TextReplyMessage> CreateReplyMessage(IConversationContext conversationContext, IDictionary<string, string> parameters)
        {
            return new TextReplyMessage
            {
                Text = "test"
            };
        }
    }

    [Route("Show test model")]
    [Route("Show test name {{value}}")]
    //[TextCompletionAction("#NAME: {{name}} , #VALUE: {{value}} , #SURNAME: {{surname}}")]
    [CompletionAction(typeof(MyFormReplyCompletionAction))]
    public class MyModel
    {
        [PropertyReply(typeof(MyModelReplyAction))]
        public string Name { get; set; }

        [PropertyReply(typeof(MyModel1ReplyAction))]
        public string Value { get; set; }

        [PropertyReply(typeof(MyModel2ReplyAction))]
        public string Surname { get; set; }
    }

    public class MyModelReplyAction : IReplyAction<TextReplyMessage>
    {
        public async Task<TextReplyMessage> CreateReplyMessage(IConversationContext conversationContext, IDictionary<string, string> parameters)
        {
            return new TextReplyMessage
            {
                Text = "What is name ?"
            };
        }
    }

    public class MyModel1ReplyAction : IReplyAction<TextReplyMessage>
    {
        public async Task<TextReplyMessage> CreateReplyMessage(IConversationContext conversationContext, IDictionary<string, string> parameters)
        {
            return new TextReplyMessage
            {
                Text = "What is value ?"
            };
        }
    }

    public class MyModel2ReplyAction : IReplyAction<TextReplyMessage>
    {
        public async Task<TextReplyMessage> CreateReplyMessage(IConversationContext conversationContext, IDictionary<string, string> parameters)
        {
            return new TextReplyMessage
            {
                Text = "What is surname ?"
            };
        }
    }

    public class MyFormReplyCompletionAction : IFormReplyCompletionAction<MyFormReplyCompletionActionData>
    {
        public async virtual Task<ReplyMessage> ExecuteAsync(IConversationContext conversationContext, MyFormReplyCompletionActionData completionActionData)
        {
            return new ReplyMessage
            {
                Text = "Form completed :)"
            };
        }
    }

    public class MyFormReplyCompletionActionData
    {    
    }
}
