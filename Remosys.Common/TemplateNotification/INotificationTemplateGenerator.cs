namespace Remosys.Common.TemplateNotification
{
    public interface INotificationTemplateGenerator
    {
        string CreateConfirmCode(ConfirmCodeTemplate confirmCode);

        string CreateInvitation(OrganizationInvitationTemplate organizationInvitationTemplate);
    }
}