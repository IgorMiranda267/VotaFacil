using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Net;
using System.Net.Mail;
using VotaFacil.Domain.Interfaces;
using System.Text;

namespace VotaFacil.Infrastructure.Service
{
    public class EmailService : IEmailService
    {
        public EmailService()
        {
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            MailMessage mail = new MailMessage("votafacil01@gmail.com", to);
            mail.Subject = "VotaFacil - Autenticação";
            mail.IsBodyHtml = true;
            mail.Body = "Olá, <br> <br> Seu código de autenticação é: "
                        + body
                        + "<br> <br> Atenciosamente, <br> Equipe VotaFacil";

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("votafacil01@gmail.com", "@4ts#g!QAJY(skd)!soi&Pq");

            client.EnableSsl = true;
            await client.SendMailAsync(mail);
        }

        public static async Task<GmailService> GetGmailServiceAsync()
        {
            var clientSecrets = new ClientSecrets
            {
                ClientId = "647755897752-2i41qp6a8oe4qcqh8j4rcst1dps14vu7.apps.googleusercontent.com",
                ClientSecret = "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"
            };

            var receiver = new LocalServerCodeReceiver();

            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                clientSecrets,
                new[] { GmailService.Scope.GmailSend },
                "user",
                CancellationToken.None,
                new FileDataStore("token.json", true),
                receiver);

            return new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "VotaFacil",
            });
        }

        public static async Task SendEmailAsyncs(string to, string subject, string body)
        {
            var gmailService = await GetGmailServiceAsync();

            // Criação da mensagem MIME manualmente
            var mimeMessage = $"From: VotaFacil <votafacil01@gmail.com>\r\n" +
                              $"To: {to}\r\n" +
                              $"Subject: =?UTF-8?B?{Convert.ToBase64String(Encoding.UTF8.GetBytes(subject))}?=\r\n" +
                              "Content-Type: text/html; charset=utf-8\r\n\r\n" +
                              $"Olá, <br><br> Seu código de autenticação é: {body}<br><br> Atenciosamente,<br>Equipe VotaFacil";

            // Codifica a mensagem para Base64URL, conforme necessário pela API Gmail
            var rawMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(mimeMessage))
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");

            var emailMessage = new Message { Raw = rawMessage };

            // Envia a mensagem
            await gmailService.Users.Messages.Send(emailMessage, "me").ExecuteAsync();
        }
    }
}
