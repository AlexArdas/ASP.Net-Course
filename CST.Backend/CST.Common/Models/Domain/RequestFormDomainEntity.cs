using System.ComponentModel.DataAnnotations.Schema;

namespace CST.Common.Models.Domain
{
    [Table("RequestForm")]
    public class RequestFormDomainEntity : IHasId
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string From { get; set; }

        public string Recipients { get; set; }

        public string Customer { get; set; }

        public DateTime ExpectedSendDate { get; set; }

        public string LinkToFilesAtOnedrive { get; set; }

        public string RequesterEmail { get; set; }

        public Guid? RequestId { get; set; }

        [ForeignKey("RequestId")]
        public virtual RequestDomainEntity Request { get; set; }
    }
}
