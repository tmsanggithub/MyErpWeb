using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRunDragon.Actions
{
    /// <summary>
    /// Summary description for Timer
    /// </summary>
    public class Timer : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(DateTime.Now.ToString("dd/MM/yyy HH:mm:ss"));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}