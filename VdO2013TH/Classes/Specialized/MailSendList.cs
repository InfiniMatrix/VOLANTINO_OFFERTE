using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using System.Data;
using System.Data.Common;

using System.Net;
using System.Net.Mail;

using VdO2013Core;
using VdO2013SRCore;
using VdO2013SRCore.Specialized;
using VdO2013THCore;
using VdO2013THCore.Specialized;

namespace VdO2013TH.Specialized
{

    public class MailSendList : ProgressItemCollection, IMailSendList
    {
        public const int DefaultSmtpPort = 25;
        public const int DefaultRetryCount = 3;
        public const int DefaultRetryDelay = 5000;

        public static ICredentialsByHost DefaultCredential
        {
            get;
            set;
        }

        private Dictionary<MailMessage, SmtpStatusCode> _messages;

        private string _smtpServer;
        private int _smtpServerPort;
        private ICredentialsByHost _credential;
        private bool _enableSSL;
        private int _retryCount;
        private int _retryDelay;
        private Exception _error;
        public String SmtpServer { get { return _smtpServer; } }
        public int SmtpServerPort { get { return _smtpServerPort; } }
        public ICredentialsByHost Credential { get { return _credential; } }
        public Boolean EnableSSL { get { return _enableSSL; } }
        public int RetryCount { get { return _retryCount; } }
        public int RetryDelay { get { return _retryDelay; } }
        public Exception Error { get { return _error; } set { _error = value; } }

        public MailSendList(String job, List<MailMessage> messages
            , String smtpServer, int smtpServerPort = DefaultSmtpPort
            , ICredentialsByHost credential = null, Boolean enableSSL = false
            , int retryCount = DefaultRetryCount, int retryDelay = DefaultRetryDelay)
            : base(job)
        {
            _smtpServer = smtpServer;
            _smtpServerPort = smtpServerPort;
            _credential = credential ?? DefaultCredential;
            _enableSSL = enableSSL;
            _retryCount = retryCount;
            _retryDelay = retryDelay;

            _messages = new Dictionary<MailMessage, SmtpStatusCode>();
            foreach (var u in messages)
            {
                _messages.Add(u, SmtpStatusCode.Ok);
            }//foreach (var u in urls)
        }
        public MailSendList(String job, MailMessage message
            , String smtpServer, int smtpServerPort = DefaultSmtpPort
            , ICredentialsByHost credential = null, Boolean enableSSL = false
            , int retryCount = DefaultRetryCount, int retryDelay = DefaultRetryDelay)
            : this(job, new List<MailMessage>() { message }, smtpServer, smtpServerPort, credential, enableSSL, retryCount, retryDelay) { }

        public Dictionary<MailMessage, SmtpStatusCode> List { get { return _messages; } }

        public Boolean ShowProgress { get; set; }
    }//public class MailSendList
}
