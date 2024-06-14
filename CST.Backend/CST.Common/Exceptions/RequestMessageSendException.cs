using System;

namespace CST.Common.Exceptions
{
    public class RequestMessageSendException : CstBaseException
    {
        public RequestMessageSendException()
        { }

        public RequestMessageSendException(string message)
            : base(message)
        { }

        public RequestMessageSendException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
