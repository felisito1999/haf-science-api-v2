using EmailService.Interfaces;
using EmailService.Models;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.Services
{
    public class EmailSenderService : IEmailSender
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailSenderService(EmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }
        public void SendEmail(Message email)
        {
            var emailMessage = CreateEmailMessage(email);

            Send(emailMessage);
        }

        public async Task SendEmailAsync(Message email)
        {
            var emailMessage = await Task.Run(() => CreateEmailMessage(email));

            await SendAsync(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            var builder = new BodyBuilder();
            string logoPath = "./Properties/Resources/tabla-periodica.png";
            var logoImage = builder.LinkedResources.Add(logoPath);
            logoImage.ContentId = MimeUtils.GenerateMessageId();
            builder.TextBody = message.Content;
            builder.HtmlBody = string
                .Format(
                @"<style>body{{ background-color: white;}} #header{{width:100%;display:flex; flex-direction:row; justify-content:center; align-items:flex-end;}}
                </style>
                <div id=""header"">
                <img width=""150px"" src=""cid:{0}""></img>
                <h1><b>Haf Science</b></h1></div><div><p>{1}</p>
                </div>",
                logoImage.ContentId, message.Content);
            
            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;

                foreach (var attachment in message.Attachments)
                {
                    using(var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    builder.Attachments.Add(attachment.FileName, 
                        fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            emailMessage.Body = builder.ToMessageBody();
            return emailMessage;
        }

        private void Send(MimeMessage emailMessage)
        {
            using(var smtpClient = new SmtpClient())
            {
                try
                {
                    smtpClient.CheckCertificateRevocation = false;
                    smtpClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
                    smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                    smtpClient.Authenticate(_emailConfiguration.UserName, _emailConfiguration.Password);

                    smtpClient.Send(emailMessage);
                }
                catch (Exception ex)
                {
                    
                    throw;
                }
                finally
                {
                    smtpClient.Disconnect(true);
                    smtpClient.Dispose();
                }
            }
        }

        private async Task SendAsync(MimeMessage email)
        {
            using(var smtpClient = new SmtpClient())
            {
                try
                {
                    smtpClient.CheckCertificateRevocation = false;
                    await smtpClient.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
                    smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                    await smtpClient.AuthenticateAsync(_emailConfiguration.UserName, _emailConfiguration.Password);

                    await smtpClient.SendAsync(email);
                }
                catch (Exception ex)
                {

                    throw;
                }
                finally
                {
                    await smtpClient.DisconnectAsync(true);
                    smtpClient.Dispose();
                }
            }
        }
    }
}
