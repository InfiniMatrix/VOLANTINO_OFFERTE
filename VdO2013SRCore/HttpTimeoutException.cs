using System;
using System.Runtime.Serialization;
using System.Net;

namespace VdO2013SRCore
{
    [Serializable]
    public class HttpTimeoutException : HttpException
    {
        public static readonly HttpStatusCode[] ErrorStatuses = { HttpStatusCode.GatewayTimeout, HttpStatusCode.RequestTimeout };
        public HttpTimeoutException(string message, HttpStatusCode statusCode = HttpStatusCode.RequestTimeout, Exception innerException = null)
            : base(message, statusCode, innerException)
        {
        }
        public HttpTimeoutException(HttpStatusCode statusCode, Uri requestedUri, Exception innerException = null)
            : base(MessageFormat.FormatWith(requestedUri, statusCode), statusCode, innerException)
        {
        }

        protected HttpTimeoutException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
            //throw new NotImplementedException();
        }
    }
}