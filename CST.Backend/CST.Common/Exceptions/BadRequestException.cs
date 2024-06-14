namespace CST.Common.Exceptions
{
    public class BadRequestException : CstBaseException
    {
        public BadRequestException()
        { }

        public BadRequestException(string message) : base(message)
        { }

        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
