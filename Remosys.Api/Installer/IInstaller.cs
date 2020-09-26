using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Remosys.Api.Core.Installer
{
    public interface IInstaller
    {
        void InstallServices(IConfiguration configuration, IServiceCollection services);
    }
}