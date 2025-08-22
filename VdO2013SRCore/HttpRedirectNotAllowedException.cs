using System;
using System.Runtime.Serialization;
using System.Net;

namespace VdO2013SRCore
{
    [Serializable]
    public class HttpRedirectNotAllowedException : HttpException
    {
        public static readonly HttpStatusCode[] ErrorStatuses = { HttpStatusCode.Ambiguous
            , HttpStatusCode.Moved
            , HttpStatusCode.MovedPermanently
            , HttpStatusCode.MultipleChoices
            , HttpStatusCode.Redirect
            , HttpStatusCode.RedirectKeepVerb
            , HttpStatusCode.RedirectMethod
            , HttpStatusCode.TemporaryRedirect };
        public HttpRedirectNotAllowedException(string message, HttpStatusCode statusCode = HttpStatusCode.Redirect, Exception innerException = null)
            : base(message, statusCode, innerException)
        {
        }

        public HttpRedirectNotAllowedException(HttpStatusCode statusCode, Uri requestedUri, Exception innerException = null)
            : base(MessageFormat.FormatWith(requestedUri, statusCode), statusCode, innerException)
        {
        }

        protected HttpRedirectNotAllowedException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
            //throw new NotImplementedException();
        }
    }// class HttpRedirectNotAllowedException
}