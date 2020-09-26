using System.Diagnostics.CodeAnalysis;
using MediatR;
using Remosys.Common.Result;
using NotImplementedException = System.NotImplementedException;

namespace Remosys.Api.Core.Application.Auth.Command.SendConfirmCode
{
    public class SendConfirmCodeCommand : IRequest<Result>
    {
        public string Mobile { get; set; }
    }
}