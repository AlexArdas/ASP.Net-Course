namespace CST.Common.Services
{
    public interface IIHubService
    {
              Task<HttpResponseMessage> CancelMailingAtIhub(Guid mailingId, string userExternalId);
    }
}
