using System.Threading.Tasks;

namespace Remosys.Common.Sms
{
    public interface IPayamakService
    {
        Task SendMessage(string receptor, string confirmCode);

        Task SendInvitation(string receptor, string organization, string link);
    }
}