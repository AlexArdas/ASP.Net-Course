using CST.Common.Exceptions;
using System;
using System.Runtime.Serialization;

namespace CST.Common
{
    [Serializable]
    public class AzureUploadException : CstBaseException
    {
        public AzureUploadException()
        { }

        public AzureUploadException(string message) : base(message)
        { }

        public AzureUploadException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}