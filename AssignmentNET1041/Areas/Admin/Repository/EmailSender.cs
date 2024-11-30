using System.Net.Mail;
using System.Net;

namespace AssignmentNET1041.Areas.Admin.Repository
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true, //bật bảo mật
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("deptraitayto123@gmail.com", "vldgbivztystcsya")
            };

            return client.SendMailAsync(
                new MailMessage(from: "deptraitayto123@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
