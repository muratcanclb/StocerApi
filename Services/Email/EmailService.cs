using Intra.Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Intra.Api.Services.DeepCell
{
    public class EmailService : IEmailService
    {
        #region Fields


        //private readonly AppSettings _appSettings;
        public IConfiguration Configuration { get; set; }

        #endregion

        #region Ctor

        public EmailService(
            IOptions<AppSettings> appSettings,
            IConfiguration configuration)
        {
            Configuration = configuration;
        }


        #endregion


        #region Methods

        public void Send(string to, string subject, string body, bool isHtml, bool widthTemplate = false)
        {
            WebRequest request = WebRequest.Create(Configuration["Email:Path"]);

            var from = new List<string>(Configuration["Email:From"].Split(","));

            List<string> _sendFrom = new List<string>();
            _sendFrom.Add("mail");

            var data = new MailModel
            {
                Id = 0,
                Subject = subject,
                To = to,
                Text = body,
                SendFrom = _sendFrom,
                TextIsHtml = isHtml
            };

            request.Method = "POST";
            var json = JsonConvert.SerializeObject(data);

            var dataBytes = Encoding.UTF8.GetBytes(json);
            int dataLength = dataBytes.Length;
            request.ContentType = "application/json";
            request.ContentLength = dataLength;
            using (var writer = request.GetRequestStream())
            {
                writer.Write(dataBytes, 0, dataLength);
            }

            WebResponse response = request.GetResponse();
            var tmp = new StreamReader(response.GetResponseStream()).ReadToEnd();



            return;
        }

        #endregion

        public class MailModel
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("subject")]
            public string Subject { get; set; }

            [JsonProperty("text")]
            public string Text { get; set; }

            [JsonProperty("to")]
            public string To { get; set; }

            [JsonProperty("sendFrom")]
            public List<string> SendFrom { get; set; }

            [JsonProperty("textIsHtml")]
            public bool TextIsHtml { get; set; }
        }


    }


    #region Methods




    #endregion

}
