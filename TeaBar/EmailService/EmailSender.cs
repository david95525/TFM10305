using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TeaBar.EmailService
{
    //寄信功能
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            System.Net.Mail.SmtpClient MySmtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
            MySmtp.Credentials = new System.Net.NetworkCredential("teabarwu@gmail.com", "Tfm103013");
            
            MySmtp.EnableSsl = true;
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;
            mail.From = new MailAddress("teabarwu@gmail.com", "TeaBarEmai");
            mail.To.Add(email);
            mail.Priority = MailPriority.Normal;
            mail.Subject = subject;
            mail.Body = htmlMessage;

            
            return MySmtp.SendMailAsync(mail);


        }
    }
}
