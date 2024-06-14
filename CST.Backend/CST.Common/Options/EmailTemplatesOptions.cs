using Microsoft.Extensions.Options;

namespace CST.Common.Options;

public class EmailTemplatesOptions
{
	public string PathToFolder { get; set; }
}

public class EmailTemplatesOptionsConfigurator : IConfigureOptions<EmailTemplatesOptions>
{
	public void Configure(EmailTemplatesOptions options)
	{
		options.PathToFolder = Path.Combine(Environment.CurrentDirectory, options.PathToFolder);
	}
}