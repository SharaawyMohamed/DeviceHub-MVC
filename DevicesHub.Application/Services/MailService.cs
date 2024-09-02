using DevicesHub.Application.Settings;
using DevicesHub.Domain.Models;
using DevicesHub.Domain.Services;
using Microsoft.Extensions.Options;
using MimeKit;

//using System.Net.Mail;
namespace DevicesHub.Application.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _options;
        public MailService(IOptions<MailSettings> options)
        {
            _options = options.Value;
        }

        public async Task SendEmailAsync(Email email)
        {

            var mailMessage = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_options.Email),
                Subject = email.Subject

            };

            mailMessage.To.Add(MailboxAddress.Parse(email.To));
            mailMessage.From.Add(new MailboxAddress(_options.DisplayName, _options.Email));

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                 <div style='font-family: Arial, sans-serif; color: #333;'>
                     <table style='width: 100%; max-width: 600px; margin: auto; border: 1px solid #e0e0e0; padding: 20px; border-radius: 5px;'>
                         <tr>
                             <td style='background-color: #f7f7f7; padding: 20px; text-align: center;'>
                                 <h1 style='color: #444;'>Welcome to Device Hub!</h1>
                             </td>
                         </tr>
                         <tr>
                             <td style='padding: 20px;'>
                                 <p>Dear {email.To},</p>
                                 <p>Thank you for registering on our website. We are thrilled to have you with us!</p>
                                 <p>If you have any questions, feel free to reach out to us at any time. We're here to help.</p>
                                 <p>Best regards,<br/>The Device Hub Team</p>
                             </td>
                         </tr>
                         <tr>
                             <td style='background-color: #f7f7f7; padding: 20px; text-align: center;'>
                                 <p style='margin: 0;'>© 2024 Device Hub. All rights reserved.</p>
                             </td>
                         </tr>
                     </table>
                 </div>",
                TextBody = $"Welcome to Device Hub!\n\nDear {email.To},\n\nThank you for registering on our website. We are thrilled to have you with us!\n\nIf you have any questions, feel free to reach out to us at any time. We're here to help.\n\nBest regards,\nThe Device Hub Team"
            };

            mailMessage.Body = bodyBuilder.ToMessageBody();

            using var smtpClient = new MailKit.Net.Smtp.SmtpClient();

            smtpClient.Connect(_options.Host, _options.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtpClient.Authenticate(_options.Email, _options.Password);

            await smtpClient.SendAsync(mailMessage);
            smtpClient.Disconnect(true);

            #region MyRegion

            //public MailService(IOptions<MailSettings> _mailSetting)
            //{
            //    mailSetting = _mailSetting.Value;
            //}
            //public void SendMail(Email email)
            //{
            //    var mail = new MimeMessage()
            //    {
            //        Sender = MailboxAddress.Parse(mailSetting.Email),
            //        Subject = email.Subject,

            //    };
            //    mail.To.Add(MailboxAddress.Parse(email.To));
            //    var builder = new BodyBuilder();
            //    builder.TextBody = email.Body;
            //    mail.Body = builder.ToMessageBody();
            //    mail.From.Add(new MailboxAddress(mailSetting.DisplayName, mailSetting.Email));

            //    using var smtp = new SmtpClient();


            //    smtp.Connect(mailSetting.Host, mailSetting.Port, MailKit.Security.SecureSocketOptions.StartTls);
            //    smtp.Authenticate(mailSetting.Email, mailSetting.Password);
            //    smtp.Send(mail);
            //    smtp.Disconnect(true); 
            #endregion

        }
    }
}
