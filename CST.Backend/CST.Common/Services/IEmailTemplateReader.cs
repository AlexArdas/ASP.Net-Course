namespace CST.Common.Services;

public interface IEmailTemplateReader
{
	public Task<string> ReadAsync<T>(string templateFileName, T embeddedModel);
}