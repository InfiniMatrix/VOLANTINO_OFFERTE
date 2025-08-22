using System;
using System.Runtime.Serialization;
using System.Net;

namespace VdO2013SRCore
{
    [Serializable]
    public class HttpNotFoundException : HttpException
    {
        public static readonly HttpStatusCode[] ErrorStatuses = { HttpStatusCode.BadGateway
            , HttpStatusCode.BadRequest
            , HttpStatusCode.Conflict
            , HttpStatusCode.ExpectationFailed
            , HttpStatusCode.Forbidden
            , HttpStatusCode.GatewayTimeout
            , HttpStatusCode.Gone
            , HttpStatusCode.HttpVersionNotSupported
            , HttpStatusCode.InternalServerError
            , HttpStatusCode.LengthRequired
            , HttpStatusCode.MethodNotAllowed
            , HttpStatusCode.NoContent
            , HttpStatusCode.NotAcceptable
            , HttpStatusCode.NotFound
            , HttpStatusCode.NotImplemented
            , HttpStatusCode.PartialContent
            , HttpStatusCode.PreconditionFailed
            , HttpStatusCode.ProxyAuthenticationRequired
            , HttpStatusCode.RequestedRangeNotSatisfiable
            , HttpStatusCode.RequestEntityTooLarge
            , HttpStatusCode.RequestTimeout
            , HttpStatusCode.RequestUriTooLong
            , HttpStatusCode.ServiceUnavailable
            , HttpStatusCode.Unauthorized
            , HttpStatusCode.UnsupportedMediaType
            , HttpStatusCode.UseProxy };
        public HttpNotFoundException(string message, HttpStatusCode statusCode = HttpStatusCode.NotFound, Exception innerException = null)
            : base(message, statusCode, innerException)
        {
        }
        public HttpNotFoundException(HttpStatusCode statusCode, Uri requestedUri, Exception innerException = null)
            : base(MessageFormat.FormatWith(requestedUri, statusCode), statusCode, innerException)
        {
        }

        protected HttpNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
            //throw new NotImplementedException();
        }
    }// class HttpNotFoundException
}