using System;
using System.Runtime.Serialization;
using System.Net;

namespace VdO2013SRCore
{
    [Serializable]
    public class HttpException : Exception
    {
        protected static readonly string MessageFormat = "Requested uri '{0}' returned code {1}.";
        public HttpStatusCode StatusCode { get; private set; }

        public HttpException(string message, HttpStatusCode statusCode, Exception innerException = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
        public HttpException(HttpStatusCode statusCode, Uri requestedUri, Exception innerException = null)
            : this(MessageFormat.FormatWith(requestedUri, statusCode), statusCode, innerException)
        {
        }

        protected HttpException(SerializationInfo serializationInfo,StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
            //throw new NotImplementedException();
        }
    }// class HttpException
}