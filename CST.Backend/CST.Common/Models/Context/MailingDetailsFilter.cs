#nullable enable

namespace CST.Common.Models.Context
{
    public class MailingDetailsFilter
    {
        public DateTime? SendOnAfter { get; set; }

        public DateTime? SendOnBefore { get; set; }

        //Search by author name, channel and subject
        public string? SearchOption { get; set; }
    }
}
