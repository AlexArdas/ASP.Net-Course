namespace CST.Common.Models.DTO;

public class MsFormRequestModel
{
	public string Name { get; set; }

	public string Description { get; set; }

	public string From { get; set; }

    public string Recipients { get; set; }

	public string Customer { get; set; }
	
	public DateTime ExpectedSendDate { get; set; }

	public string LinkToFilesAtOnedrive { get; set; }

	public string RequesterEmail { get; set; }
}