using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MiniCrmApi.Services.Abstract;

namespace MiniCrmApi.Services.Concrete;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _config;
    public SmtpEmailService(IConfiguration config)
    {
        _config = config;
    }
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var smtpSection = _config.GetSection("Smtp");

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("MiniCrm", smtpSection["From"]));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = subject;

        var multipart = new MultipartAlternative();
        multipart.Add(new TextPart("plain") { Text = body });
        multipart.Add(new TextPart("html") { Text = body });
     
        message.Body = multipart;
            
        using var client = new SmtpClient();
        await client.ConnectAsync( smtpSection["Host"],int.Parse(smtpSection["Port"]!),SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(smtpSection["Username"], smtpSection["Password"]);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
