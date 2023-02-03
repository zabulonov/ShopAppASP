using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;
//why this thing dosn't work?
namespace App_course.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string body)
        {
            MailjetClient client = new MailjetClient("af1bdf08a6507fc2f4c6e02b9db374d4", "03d570a1466d0f8e826bd9c9ff034da8")
            {
                //Version = ApiVersion.V3_1,
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",
       new JObject {
        {"Email", "zabulonov444@yandex.ru"},
        {"Name", "Alexey"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          email
         }, {
          "Name",
          "Alexey"
         }
        }
       }
      }, {
       "Subject",
       subject
      },  {
       "HTMLPart",
       body
      },
     }
             });
            await client.PostAsync(request);
        }
    }
}
