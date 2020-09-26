using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Remosys.Common.TemplateNotification;
using Serilog;

namespace Remosys.Common.Sms
{
    public class PayamakService : IPayamakService
    {
        private readonly IOptionsMonitor<GhasedakService> _ghasedakService;
        private readonly INotificationTemplateGenerator _notificationTemplateGenerator;
        public PayamakService(IOptionsMonitor<GhasedakService> ghasedakService, INotificationTemplateGenerator notificationTemplateGenerator)
        {
            _ghasedakService = ghasedakService;
            _notificationTemplateGenerator = notificationTemplateGenerator;
        }

        public async Task SendMessage(string receptor, string confirmCode)
        {
            var sms = new Ghasedak.Core.Api(_ghasedakService.CurrentValue.ApiKey);

            try
            {
                await sms.SendSMSAsync(_notificationTemplateGenerator.CreateConfirmCode(new ConfirmCodeTemplate { Code = confirmCode }), receptor, _ghasedakService.CurrentValue.LineNumber);
            }
            catch (Exception e)
            {
                Log.Error(e.StackTrace, e.Message);
            }
        }

        public async Task SendInvitation(string receptor, string organization, string link)
        {
            var sms = new Ghasedak.Core.Api(_ghasedakService.CurrentValue.ApiKey);
            try
            {
                await sms.SendSMSAsync(_notificationTemplateGenerator.CreateInvitation(new OrganizationInvitationTemplate { Link = link, Organization = organization }), receptor, _ghasedakService.CurrentValue.LineNumber);
            }
            catch (Exception e)
            {
                Log.Error(e.StackTrace, e.Message);
            }
        }
    }
}