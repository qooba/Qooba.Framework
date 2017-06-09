using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Abstractions.Models.Attachments;
using Qooba.Framework.Bot.Abstractions.Models.Templates;
using System.Collections.Generic;
using System.Linq;

namespace Qooba.Framework.Bot
{
    public class CarouselReplyBuilder : IReplyBuilder<CarouselReplyMessage>
    {
        public async Task<ReplyMessage> ExecuteAsync(IConversationContext context, CarouselReplyMessage reply)
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
                                Buttons = x.Buttons.Select(b => b.ToButton()).ToList()
                            }
                        ).ToList()
                    }
                }
            };
        }
    }

    public class CarouselReplyMessage
    {
        public IList<CarouselReplyElement> Elements { get; set; }
    }

    public class CarouselReplyElement
    {
        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Image { get; set; }

        public string DefaultActionUrl { get; set; }

        public IList<FullButton> Buttons { get; set; }
    }
}
