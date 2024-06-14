namespace CST.Common.Models.DTO
{
    public class RequestMessagesWithFormResponse
    {
        public RequestFormResponse RequestFormResponse { get; set; }

        public List<RequestMessageResponse> RequestMessagesResponse { get; set; }
    }
}
