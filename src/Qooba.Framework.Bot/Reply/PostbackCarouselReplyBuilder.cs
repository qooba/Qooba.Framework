using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Abstractions.Models.Attachments;
using Qooba.Framework.Bot.Abstractions.Models.Templates;
using Qooba.Framework.Bot.Abstractions.Models.Buttons;
using System.Collections.Generic;
using System.Linq;

namespace Qooba.Framework.Bot
{
    public class PostbackCarouselReplyBuilder : IReplyBuilder<PostbackCarouselReplyMessage>
    {
        public async Task<ReplyMessage> BuildAsync(IConversationContext context, PostbackCarouselReplyMessage reply)
        {
            return new ReplyMessage
            {
                Attachment = new TemplateAttachment
                {
                    Payload = new GenericTemplateAttachmentPayload
                    {
                        Elements = reply.Elements.Select(x =>
                            new Element
                            {
                                Title = x.Title,
                                Subtitle = x.Subtitle,
                                Image_url = x.Image,
                                Default_action = !string.IsNullOrEmpty(x.DefaultActionUrl) ? new DefaultAction { Url = x.DefaultActionUrl } : null,
                                Buttons = x.Buttons.Cast<Button>().ToList()
                            }
                        ).ToList()
                    }
                }
            };
        }
    }

    public class PostbackCarouselReplyMessage
    {
        public IList<PostbackCarouselReplyElement> Elements { get; set; }
    }

    public class PostbackCarouselReplyElement
    {
        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Image { get; set; }

        public string DefaultActionUrl { get; set; }

        public IList<PostbackButton> Buttons { get; set; }
    }
}
