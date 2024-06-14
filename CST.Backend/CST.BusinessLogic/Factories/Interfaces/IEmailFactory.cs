using CST.Common.Models.Messages;

namespace CST.BusinessLogic.Factories.Interfaces;

public interface IEmailFactory
{
	Email Create(string userEmail, string subject, string bodyHtml);
	Email Create(Guid emailId, string userEmail, string subject, string bodyHtml);
}