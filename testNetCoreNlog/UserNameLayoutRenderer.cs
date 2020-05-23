using System;
using System.Text;
using NLog;
using NLog.LayoutRenderers;

namespace testNetCoreNlog
{
    [LayoutRenderer("user-name")]
    public class UserNameLayoutRenderer :  LayoutRenderer
    {
        private const string Key = "userName";

        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            var userName = default(string);

            try
            {
                userName = CallContext<string>.GetData(Key);
            }
            finally
            {
                // 没有的一律给 "_thread"
                if (string.IsNullOrWhiteSpace(userName))
                {
                    userName = "_thread";
                }
            }
            
            builder.Append(userName);
        }
        
        public static void Set(string userName)
        {
            Console.WriteLine("----------- append user name set -----------");
            CallContext<string>.SetData(Key, userName);
        }
    }
}