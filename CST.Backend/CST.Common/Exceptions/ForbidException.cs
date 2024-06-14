namespace CST.Common.Exceptions
{
    public class ForbidException : CstBaseException
    {
        public ForbidException()
        { }

        public ForbidException(string message)
            : base(message)
        { }

        public ForbidException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
