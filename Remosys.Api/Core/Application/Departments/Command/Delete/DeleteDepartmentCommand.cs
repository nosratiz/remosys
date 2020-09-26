using System;
using MediatR;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Departments.Command.Delete
{
    public class DeleteDepartmentCommand : IRequest<Result>
    {
        public Guid Id { get; set; }

    }
}