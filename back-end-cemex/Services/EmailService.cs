using back_end_cemex.Entities;
using back_end_cemex.Helpers.Email;
using back_end_cemex.Helpers.Email.ForgotPassword;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.IO;

namespace back_end_cemex.Services
{
    public class EmailService : IEmailService
    {
        EmailSettings _emailSettings = null;
        private readonly IConfiguration configuration;

        public EmailService(IOptions<EmailSettings> options, IConfiguration configuration)
        {
            _emailSettings = options.Value;
            this.configuration = configuration;
        }
        public bool SendEmail(EmailData emailData)
        {
            try
            {
                MimeMessage emailMessage = new MimeMessage();
                MailboxAddress emailFrom = new MailboxAddress(_emailSettings.Name, _emailSettings.EmailId);
                emailMessage.From.Add(emailFrom);
                MailboxAddress emailTo = new MailboxAddress(emailData.EmailToName, emailData.EmailToId);
                emailMessage.To.Add(emailTo);
                emailMessage.Subject = emailData.EmailSubject;
                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.TextBody = emailData.EmailBody;
                emailMessage.Body = emailBodyBuilder.ToMessageBody();
                SmtpClient emailClient = new SmtpClient();
                emailClient.Connect(_emailSettings.Host, _emailSettings.Port, _emailSettings.UseSSL);
                emailClient.Authenticate(_emailSettings.EmailId, _emailSettings.Password);
                emailClient.Send(emailMessage);
                emailClient.Disconnect(true);
                emailClient.Dispose();
                return true;
            }
            catch (Exception)
            {
                //Log Exception Details
                return false;
            }
        }

        public bool SendUserEmail(UserForgotPassword userForgotPassword)
        {
            try
            {
                MimeMessage emailMessage = new MimeMessage();

                MailboxAddress emailFrom = new MailboxAddress(_emailSettings.Name, _emailSettings.EmailId);
                emailMessage.From.Add(emailFrom);

                MailboxAddress emailTo = new MailboxAddress(userForgotPassword.FirstName, userForgotPassword.Email);
                emailMessage.To.Add(emailTo);

                emailMessage.Subject = "Recuper Contraseña";

                string FilePath = Directory.GetCurrentDirectory() + "\\Helpers\\Email\\Templates\\ForgotPassword\\ForgotPassword.html";
                string EmailTemplateText = File.ReadAllText(FilePath);

                EmailTemplateText = string.Format(EmailTemplateText, userForgotPassword.FirstName, userForgotPassword.LastName, userForgotPassword.TokenURL, configuration["urllogo"]);

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = EmailTemplateText;
                emailMessage.Body = emailBodyBuilder.ToMessageBody();

                SmtpClient emailClient = new SmtpClient();
                emailClient.Connect(_emailSettings.Host, _emailSettings.Port, _emailSettings.UseSSL);
                emailClient.Authenticate(_emailSettings.EmailId, _emailSettings.Password);
                emailClient.Send(emailMessage);
                emailClient.Disconnect(true);
                emailClient.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                //Log Exception Details
                return false;
            }
        }

        public bool SendEmailWithAttachment(EmailDataWithAttachment emailData)
        {
            try
            {
                MimeMessage emailMessage = new MimeMessage();

                MailboxAddress emailFrom = new MailboxAddress(_emailSettings.Name, _emailSettings.EmailId);
                emailMessage.From.Add(emailFrom);

                MailboxAddress emailTo = new MailboxAddress(emailData.EmailToName, emailData.EmailToId);
                emailMessage.To.Add(emailTo);

                emailMessage.Subject = emailData.EmailSubject;

                BodyBuilder emailBodyBuilder = new BodyBuilder();

                if (emailData.EmailAttachments != null)
                {
                    byte[] attachmentFileByteArray;
                    foreach (IFormFile attachmentFile in emailData.EmailAttachments)
                    {
                        if (attachmentFile.Length > 0)
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                attachmentFile.CopyTo(memoryStream);
                                attachmentFileByteArray = memoryStream.ToArray();
                            }
                            emailBodyBuilder.Attachments.Add(attachmentFile.FileName, attachmentFileByteArray, ContentType.Parse(attachmentFile.ContentType));
                        }
                    }
                }

                emailBodyBuilder.TextBody = emailData.EmailBody;
                emailMessage.Body = emailBodyBuilder.ToMessageBody();

                SmtpClient emailClient = new SmtpClient();
                emailClient.Connect(_emailSettings.Host, _emailSettings.Port, _emailSettings.UseSSL);
                emailClient.Authenticate(_emailSettings.EmailId, _emailSettings.Password);
                emailClient.Send(emailMessage);
                emailClient.Disconnect(true);
                emailClient.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                //Log Exception Details
                return false;
            }
        }
    }

}
