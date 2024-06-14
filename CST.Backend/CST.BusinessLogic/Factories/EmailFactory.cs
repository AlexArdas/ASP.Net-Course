using CST.BusinessLogic.Factories.Interfaces;
using CST.Common.Models.Messages;

namespace CST.BusinessLogic.Factories;

public class EmailFactory : IEmailFactory
{
	public Email Create(Guid emailId, string userEmail, string subject, string bodyHtml) =>
		new()
		{
			Id = emailId,
			To = userEmail,
			Subject = subject,
			BodyHtml = bodyHtml
		};

	public Email Create(string userEmail, string subject, string bodyHtml) =>
		Create(Guid.NewGuid(), userEmail, subject, bodyHtml);
}