using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace covidance.Lib.Communications
{
    public interface IMyEmailSender
    {
        Task<dynamic> SendEmailAsync(string mailkey, string from, string to, string subject, string messageHtml, string messageText);
    }
}
