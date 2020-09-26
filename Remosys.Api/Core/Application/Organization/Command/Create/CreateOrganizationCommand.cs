using System;
using MediatR;
using Remosys.Api.Core.Application.Organization.Dto;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Organization.Command.Create
{
    public class CreateOrganizationCommand : IRequest<Result<OrganizationDto>>
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}