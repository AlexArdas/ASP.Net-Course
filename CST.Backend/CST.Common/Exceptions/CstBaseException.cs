namespace CST.Common.Exceptions
{
    public class CstBaseException : Exception
    {
        public CstBaseException()
        { }

        public CstBaseException(string message)
            : base(message)
        { }

        public CstBaseException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }

}
