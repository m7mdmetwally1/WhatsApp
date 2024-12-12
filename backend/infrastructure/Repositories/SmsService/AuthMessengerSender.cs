using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using   infrastructure.Repositories.SmsService;
using Microsoft.AspNetCore.Identity.UI.Services;
using Twilio.Clients;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Application.Interfaces;



namespace infrastructure.Repositories.SmsService;

    
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
    private readonly ILogger<AuthMessageSender> _logger;

    public AuthMessageSender(IOptions<SMSoptions> optionsAccessor,ILogger<AuthMessageSender> logger)
        {
            Options = optionsAccessor.Value;
            this._logger = logger;
        }

        public SMSoptions Options { get; }  

        public Task SendEmailAsync(string email, string subject, string messageBody)
        {
           
                    var message = new MimeMessage();
            message.From.Add(new MailboxAddress("WhatsApp", "mohamedmetwallyrmdan@gmail.com"));
            message.To.Add(new MailboxAddress(subject, email));
            message.Subject = subject;
        
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = messageBody;

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                client.Authenticate("mohamedmetwallyrmdan@gmail.com", "ptxj xbdt iulq xspr");
                client.Send(message);
                client.Disconnect(true);
            }
            
            return Task.FromResult(0);
        }

        public async Task<bool> SendSmsAsync(string number, string message)
        {
         
            var accountSid = Options.SMSAccountIdentification;
          
            var authToken = Options.SMSAccountPassword;            

            var client = new TwilioRestClient(accountSid, authToken);
                 
            TwilioClient.Init(accountSid, authToken);
         
            try
            {
                var messageResource =await  MessageResource.CreateAsync(
                to: new Twilio.Types.PhoneNumber($"whatsapp:{number}"),
                from: new Twilio.Types.PhoneNumber("whatsapp:+17754875250"),
                body: message,
                client: client);

                if (messageResource.Status == MessageResource.StatusEnum.Queued ||
                messageResource.Status == MessageResource.StatusEnum.Sent ||
                messageResource.Status == MessageResource.StatusEnum.Delivered)
                {
                    return true; 
                }

                return false;
            }catch(Exception ex)
            {
                _logger.LogInformation($"error message {ex}");
                return false;
            }             
            
            }
            
    }

    

   // return MessageResource.CreateAsync(
            //   to: new PhoneNumber(number),
            //   from: new PhoneNumber(Options.SMSAccountFrom),
            //   body: message
            //   );