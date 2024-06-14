using System;
using CST.BusinessLogic.Factories;
using CST.Common.Models.Messages;
using CST.Tests.Common;
using FluentAssertions;
using Xunit;

namespace CST.BusinessLogic.Tests;

public class EmailFactoryTests : AutoMockerTestsBase<EmailFactory>
{
	private const string requesterEmail = "email to";
	private const string emailSubject = "email subject";
	private const string emailBody = "email body";

	[Fact]
	public void CreateEmail_ShouldReturnEmail_WithoutInputEmailId()
	{
		// Arrange
		// Act
		var actualEmail = Target.Create(requesterEmail, emailSubject, emailBody);

		// Assert
		AssertEmail(actualEmail);
	}

	[Fact]
	public void CreateEmail_ShouldReturnEmail_WithInputEmailId()
	{
		// Arrange
		var emailId = Guid.NewGuid();

		// Act
		var actualEmail = Target.Create(emailId, requesterEmail, emailSubject, emailBody);

		// Assert
		actualEmail.Id.Should().Be(emailId);
		AssertEmail(actualEmail);
	}

	private void AssertEmail(Email actualEmail)
	{
		actualEmail.Id.Should().NotBeEmpty();
		actualEmail.Subject.Should().Be(emailSubject);
		actualEmail.To.Should().Be(requesterEmail);
		actualEmail.BodyHtml.Should().Be(emailBody);
	}
}