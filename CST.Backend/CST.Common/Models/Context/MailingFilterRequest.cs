using CST.Common.Models.Enums;

namespace CST.Common.Models.Context
{
    public class MailingFilterRequest
    {
        public string AuthorSearchOption { get; set; }

        public string SubjectSearchOption { get; set;}

        public string ChannelSearchOption { get; set; }

        public DateTime? SendOnAfter { get; set; }

        public DateTime? SendOnBefore { get; set; }

        public List<Guid> MailingLocations { get; set; }

        public List<MailingStatus> IncludedMailingStatuses { get; set; }

        public List<MailingStatus> ExcludedMailingStatuses { get; set; }

    }
}
