using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITHelpDesk.Utils
{
    public class MailUtil
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MailUtil));

        public static void SendMail(string subject, string content, string to, string cc, string bcc)
        {
            
        }
    }
}