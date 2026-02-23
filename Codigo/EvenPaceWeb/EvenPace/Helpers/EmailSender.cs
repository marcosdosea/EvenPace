using System.Net;
using System.Net.Mail;
using Castle.Core.Smtp;


namespace EvenPaceWeb.Helpers;

public class EmailSender : IEmailSender
{
    private readonly SmtpClient _client;
    private readonly string _from;
    private readonly string _webRootPath;
    
    public EmailSender(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _from = configuration["Smtp:From"];
        
        _webRootPath = environment.WebRootPath;

        _client = new SmtpClient
        {
            Host = configuration["Smtp:Host"],
            Port = int.Parse(configuration["Smtp:Port"]),
            Credentials = new NetworkCredential(
                configuration["Smtp:User"],
                configuration["Smtp:Pass"]
            ),
            EnableSsl = true
        };
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MailMessage(_from, email)
        {
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true
        };
        
        await _client.SendMailAsync(message);
    }

    public void Send(string from, string to, string subject, string messageText)
    {
        throw new NotImplementedException();
    }

    public void Send(MailMessage message)
    {
        throw new NotImplementedException();
    }

    public void Send(IEnumerable<MailMessage> messages)
    {
        throw new NotImplementedException();
    }
}