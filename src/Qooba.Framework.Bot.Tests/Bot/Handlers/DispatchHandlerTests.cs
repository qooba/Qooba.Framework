﻿using Moq;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Context;
using Qooba.Framework.Bot.Handlers;
using Xunit;

namespace Qooba.Framework.Bot.Tests.Handlers
{
    public class DispatchHandlerTests
    {
        private IHandler dispatchHandler;

        private Mock<IDispatcher> dispatcherMock;
        
        public DispatchHandlerTests()
        {
            this.dispatcherMock = new Mock<IDispatcher>();
            this.dispatchHandler = new DispatchHandler(this.dispatcherMock.Object);
        }

        [Fact]
        public void DispatchHandlingTest()
        {
            var reply = new Reply
            {
                Message = new ReplyMessage
                {
                    Text = "hello"
                }
            };

            IConversationContext context = new ConversationContext
            {
                Reply = reply
            };

            this.dispatchHandler.InvokeAsync(context).Wait();

            this.dispatcherMock.Verify(x => x.SendAsync(reply), Times.Once);
        }
    }
}