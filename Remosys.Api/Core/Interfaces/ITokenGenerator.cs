using System.Threading;
using System.Threading.Tasks;
using Remosys.Api.Core.Application.Auth.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Interfaces
{
    public interface ITokenGenerator
    {
        Task<Result<AuthenticationResult>> Generate(User user, CancellationToken cancellationToken);
    }
}