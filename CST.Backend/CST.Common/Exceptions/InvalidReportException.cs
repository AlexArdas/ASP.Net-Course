namespace CST.Common.Exceptions
{
    public class InvalidReportException : CstBaseException
    {
        public InvalidReportException()
        { }

        public InvalidReportException(string message)
            : base(message)
        { }

        public InvalidReportException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
