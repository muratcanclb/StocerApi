using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intra.Api.Services.DeepCell
{
    public interface IEmailService
    {
        void Send(string to, string subject, string body, bool isHtml, bool widthTemplate = false);
    }
}
