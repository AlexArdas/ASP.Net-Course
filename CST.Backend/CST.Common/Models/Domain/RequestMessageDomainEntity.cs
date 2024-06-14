using System.ComponentModel.DataAnnotations.Schema;

namespace CST.Common.Models.Domain
{
    [Table("RequestMessage")]
    public class RequestMessageDomainEntity : IHasId
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid RequestId { get; set; }
        public RequestDomainEntity Request { get; set; }
        public DateTime? SendOn { get; set; }
    }
}
