using Moq;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Form;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Form;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Qooba.Framework.Bot.Tests
{
    public class FormReplyBuilderTests
    {
        private IReplyBuilder<FormReplyMessage> replyBuilder;

        private Mock<IConversationContext> conversationContextMock;

        private Mock<IReplyFactory> replyFactoryMock;

        private Mock<IGenericExpressionFactory> genericExpressionFactoryMock;

        public FormReplyBuilderTests()
        {
            this.conversationContextMock = new Mock<IConversationContext>();
            this.replyFactoryMock = new Mock<IReplyFactory>();
            this.genericExpressionFactoryMock = new Mock<IGenericExpressionFactory>();
            var routers = new IRouter[] { };
            Func<object, IFormReplyCompletionAction> formReplyCompletionActionFunc = x => (IFormReplyCompletionAction)new TextFormReplyCompletionAction();
            this.replyBuilder = new FormReplyBuilder(this.replyFactoryMock.Object, routers, this.genericExpressionFactoryMock.Object, formReplyCompletionActionFunc);

            this.replyFactoryMock.Setup(x => x.CreateReplyAsync(It.IsAny<IConversationContext>(), It.IsAny<ReplyItem>())).Returns(Task.FromResult(new Reply
            {
                Message = new ReplyMessage
                {
                    Text = "hello"
                }
            }));
        }

        [Fact]
        public void FirstPropertyTest()
        {
            PrepareRoutes();
            var reply = PrepareFormReplyMessage();

            var replyMessage = this.replyBuilder.ExecuteAsync(this.conversationContextMock.Object, reply).Result;

            Assert.True(replyMessage.Text == "hello");
        }

        [Fact]
        public void SecondPropertyTest()
        {
            var routeData = new Dictionary<string, object>();
            PrepareRoutes(routeData);
            PrepareReply();
            this.conversationContextMock.Setup(x => x.Entry).Returns(new Entry
            {
                Message = new Messaging
                {
                    Message = new EntryMessage
                    {
                        Text = "MyFirstName"
                    }
                }
            });

            var reply = PrepareFormReplyMessage();

            var replyMessage = this.replyBuilder.ExecuteAsync(this.conversationContextMock.Object, reply).Result;

            Assert.True(replyMessage.Text == "hello");
            Assert.True(routeData["FirstName"].ToString() == "MyFirstName");
        }

        [Fact]
        public void ThirdPropertyTest()
        {
            var routeData = new Dictionary<string, object>() { { "FirstName", "MyFirstName" } };
            PrepareRoutes(routeData);
            PrepareReply();
            this.conversationContextMock.Setup(x => x.Entry).Returns(new Entry
            {
                Message = new Messaging
                {
                    Message = new EntryMessage
                    {
                        Text = "MyLastName"
                    }
                }
            });
            var reply = PrepareFormReplyMessage();

            var replyMessage = this.replyBuilder.ExecuteAsync(this.conversationContextMock.Object, reply).Result;

            Assert.True(replyMessage.Text == "hello");
            Assert.True(routeData["LastName"].ToString() == "MyLastName");
        }

        private void PrepareReply()
        {
            this.conversationContextMock.Setup(x => x.Reply).Returns(new Reply
            {
                Message = new ReplyMessage
                {
                    Text = "What is your first name ?"
                }
            });
        }

        private void PrepareRoutes(IDictionary<string, object> routeData = null)
        {
            this.conversationContextMock.Setup(x => x.Route).Returns(new Route
            {
                RouteData = routeData ?? new Dictionary<string, object>()
            });
        }

        private static FormReplyMessage PrepareFormReplyMessage()
        {
            return new FormReplyMessage
            {
                Properties = new[]
                            {
                    new FormReplyMessageProperty
                    {
                        PropertyName = "FirstName",
                        PropertyType = "string",
                        ReplyItem = new ReplyItem
                        {
                            ReplyType = "text",
                            Reply = new TextReplyMessage
                            {
                                Text = "What is your first name ?"
                            }
                        }
                    },
                    new FormReplyMessageProperty
                    {
                        PropertyName = "LastName",
                        PropertyType = "string",
                        ReplyItem = new ReplyItem
                        {
                            ReplyType = "text",
                            Reply = new TextReplyMessage
                            {
                                Text = "What is your last name ?"
                            }
                        }
                    },
                    new FormReplyMessageProperty
                    {
                        PropertyName = "Address",
                        PropertyType = "string",
                        ReplyItem = new ReplyItem
                        {
                            ReplyType = "text",
                            Reply = new TextReplyMessage
                            {
                                Text = "What is your address ?"
                            }
                        }
                    }
                }
            };
        }
    }
}
