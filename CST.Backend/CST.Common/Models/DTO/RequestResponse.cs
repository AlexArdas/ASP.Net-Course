using CST.Common.Models.Enums;

namespace CST.Common.Models.DTO
{
    public class RequestResponse
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }

        public UserBriefResponse Requester {get; set; }

        public UserBriefResponse Assignee { get; set; }

        public RequestStatus RequestStatus { get; set; }
    }
}
