using MailKit.Net.Smtp;
using MimeKit;
using plannerBackEnd.Common.Constants;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace plannerBackEnd.Common.EmailManager
{
    public class EmailSender
	{
		public class EmailMessage
		{
			public string From { get; set; } = "";
			public string To { get; set; } = "";
			public string Subject { get; set; } = "";
			public string Body { get; set; } = "";
			public string SchemaName { get; set; } = "";
			public string UserName { get; set; } = "";
			public int CustomerId { get; set; } = 0;

			public IEnumerable<EmailAttachment> Attachments = null;
		}

		public static ConcurrentQueue<EmailMessage> emailQueue = new ConcurrentQueue<EmailMessage>();

		private readonly IServiceProvider serviceProvider;

        public EmailSender(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
        }

		// -------------------------------------------------------------------------------------------------
        public void Send (EmailMessage emailMessage)
        {
            // Convert attachment content to memory stream
            if (emailMessage.Attachments != null)
            {
                foreach (EmailAttachment attachment in emailMessage.Attachments)
                {
                    attachment.Stream = new MemoryStream(attachment.Content);
                }
            }

            MimeMessage mimeMessage = new MimeMessage();
            setupFrom(emailMessage.From, mimeMessage);
            setupTo(emailMessage.To, emailMessage.Subject, mimeMessage);
            mimeMessage.Subject = emailMessage.Subject;
            BodyBuilder bodyBuilder = new BodyBuilder {HtmlBody = emailMessage.Body};
            setupAttachments(emailMessage.Attachments, bodyBuilder);
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            send(mimeMessage);
        }

        // ---------------------------------------------------------------------------------------------

		private void setupFrom (string messageFrom, MimeMessage message)
		{
			if (string.IsNullOrEmpty(messageFrom))
				messageFrom = ApplicationConstants.PublicEmail;
			messageFrom = messageFrom.Replace("\r", "");
			message.From.Add(new MailboxAddress("The StudyBuddy Team", messageFrom));
		}

		// ---------------------------------------------------------------------------------------------

        private void setupTo(string messageTo, string subject, MimeMessage message)
        {
            char[] delimiterChars = {';'};
            string[] recipients = messageTo.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < recipients.GetLength(0); i++)
            {

                if (recipients[i].Trim().Length > 0)
                {
                    message.To.Add(new MailboxAddress("", recipients[i]));
                }

            }
        }

        // ---------------------------------------------------------------------------------------------
		private void setupAttachments(IEnumerable<EmailAttachment> attachments, BodyBuilder bodyBuilder)
		{
			if (attachments != null)
			{
				foreach (EmailAttachment attachment in attachments)
				{
					bodyBuilder.Attachments.Add(attachment.FileName, attachment.Stream);
				}
			}
		}

		// ---------------------------------------------------------------------------------------------
		private void send(MimeMessage message)
		{
			bool success = false;
			Exception exception = null;
			for (int i = 0; i < ApplicationConstants.MaxSendRetries && !success; i++)
			{
				try
				{
					//Be careful that the SmtpClient class is the one from Mailkit not the framework!
					using (var emailClient = new SmtpClient())
					{
						emailClient.Connect(ApplicationConstants.GmailSMTPServer, Convert.ToInt32(ApplicationConstants.GmailSMTPPort), false);  //The last parameter here is to use SSL
						emailClient.AuthenticationMechanisms.Remove("XOAUTH2");   //Remove any OAuth functionality as we won't be using it. 
                        emailClient.Authenticate(ApplicationConstants.GmailSMTPUser, ApplicationConstants.GmailSMTPPassword);
                        emailClient.Send(message);
						emailClient.Disconnect(true);
					}
					success = true;
                }
				catch (Exception exc)
				{
					exception = exc;
					Thread.Sleep(2000);
				}
			}

			if (!success)
				throw exception;
		}

        /*// ---------------------------------------------------------------------------------------------
        private string getAccessToken()
        {
            var certificate = new X509Certificate2(@"C:\path\to\certificate.p12", "password", X509KeyStorageFlags.Exportable);
            var credential = new ServiceAccountCredential(new ServiceAccountCredential
                .Initializer("your-developer-id@developer.gserviceaccount.com")
                {
                    // Note: other scopes can be found here: https://developers.google.com/gmail/api/auth/scopes
                    Scopes = new[] { "https://mail.google.com/" },
                    User = "username@gmail.com"
                }.FromCertificate(certificate));

            //You can also use FromPrivateKey(privateKey) where privateKey
            // is the value of the field 'private_key' in your serviceName.json file

            bool result = await credential.RequestAccessTokenAsync(cancel.Token);

            // Note: result will be true if the access token was received successfully
            return credential.Token.AccessToken;
        }*/
	}
}