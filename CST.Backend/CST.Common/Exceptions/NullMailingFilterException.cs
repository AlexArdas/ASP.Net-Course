namespace CST.Common.Exceptions
{
    public class NullMailingFilterException : CstBaseException
    {
        public NullMailingFilterException()
        { }

        public NullMailingFilterException(string message)
            : base(message)
        { }

        public NullMailingFilterException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
