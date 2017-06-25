using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Qooba.Framework.Bot.Form;

namespace Qooba.Framework.Bot.Tests.Bot.Form
{
    public class FormReplyActionTests
    {
        public FormReplyActionTests()
        {

        }

        [Fact]
        public void CreateReplyMessageTest()
        {
            var action = new FormReplyAction<TestModel>();
            var replyMessage = action.CreateReplyMessage(null, null).Result;
        }
    }
}
