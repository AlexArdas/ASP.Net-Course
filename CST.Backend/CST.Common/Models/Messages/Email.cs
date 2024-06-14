using CST.Common.Attributes;
using System;

namespace CST.Common.Models.Messages
{
    [AzureMessage("email", true)]
    public class Email : MessageBase
    {
        public Guid Id { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }

        public string BodyHtml { get; set; }

    }
}
