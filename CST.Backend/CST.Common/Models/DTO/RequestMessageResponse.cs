namespace CST.Common.Models.DTO
{
    public class RequestMessageResponse
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid RequestId { get; set; }
    }
}
