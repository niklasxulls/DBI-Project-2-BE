using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using stackblob.Application.Interfaces.Services;
using stackblob.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Infrastructure.Services;

public class MailService : IMailService
{
    private readonly MailSettings _mailSettings;
    private readonly GlobalSettings _globalSettings;
    private readonly IHttpContextAccessor _httpContext;

    public string TemplatePath => Path.Combine(Environment.CurrentDirectory, _mailSettings.TemplatePath);

    private const string RECEIVER_IDENTIFIER = "{{Receiver}}";
    private const string SENDER_IDENTIFIER = "{{Sender}}";
    private const string ACCEPT_BUTTON_LINK_IDENTIFIER = "{{AcceptButtonLink}}";


    public MailService(IOptions<MailSettings> mailSettings,IMapper mapper, IHttpContextAccessor httpContext, IOptions<GlobalSettings> globalSettings)
    {
        _mailSettings = mailSettings.Value;
        _globalSettings = globalSettings.Value;
        _httpContext = httpContext;
    }

    private const string USER_VERIFICATION_FILENAME = "SendUserEmailVerification.html";

    public async Task<bool> SendEmailVerification(User user, Guid verificationGuid)
    {
        MailMessage message = new();

        message.To.Add(new MailAddress(user.Email));
        message.Subject = string.Format($"{user.Firstname} verify your Stackblob email!");

        var template = Path.Combine(TemplatePath, USER_VERIFICATION_FILENAME);
        if (!File.Exists(template))
            throw new FileNotFoundException($"Did not find {template}");

        var body = "";

        using (var reader = new StreamReader(template))
        {
            body = reader.ReadToEnd();
            body = body.Replace(RECEIVER_IDENTIFIER, user.Name);
            body = body.Replace(SENDER_IDENTIFIER, "Stackblob");
            //TODO: implement dynamic routing for email verfication route -> declare route url itself in globalSettings section
            body = body.Replace(ACCEPT_BUTTON_LINK_IDENTIFIER, _globalSettings.ApplicationUrl + $"/auth/verify-email/{verificationGuid}");
        }

        
        message.Body = body;
        message.IsBodyHtml= true;

        try
        {
            await SendEmail(message);
            return true;
        } catch (Exception ex)
        {
            return false;
        }
    }


    public async Task SendEmail(MailMessage msg)
    {
        msg.From = new MailAddress(_mailSettings.Username, _mailSettings.From);

        using var client = new SmtpClient(_mailSettings.Host, _mailSettings.Port);
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.EnableSsl = true;
        client.UseDefaultCredentials = false;
        client.Timeout = 5000;

        client.Credentials = new NetworkCredential(_mailSettings.Username, _mailSettings.AppPassword);

        //client.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);

        //client.Authenticate(_mailSettings.Username, _mailSettings.Password);

        await client.SendMailAsync(msg);

        //client.Disconnect(true);
    }




}
