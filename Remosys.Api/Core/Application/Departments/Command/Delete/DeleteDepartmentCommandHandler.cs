using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Claims;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Departments.Command.Delete
{
    public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, Result>
    {
        private readonly IMongoRepository<Department> _departmentRepository;
        private readonly ICurrentUserService _currentUserService;

        public DeleteDepartmentCommandHandler(IMongoRepository<Department> departmentRepository, ICurrentUserService currentUserService)
        {
            _departmentRepository = departmentRepository;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.GetAsync(request.Id, cancellationToken);

            if (department is null)
                return Result.Failed(
                    new NotFoundObjectResult(new ApiMessage(ResponseMessage.DepartmentNotFound)));

            if (_currentUserService.RoleName == Role.Manager && department.Manager.Id != Guid.Parse(_currentUserService.UserId))
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(ResponseMessage.DeleteNotAllowed)));
            

            await _departmentRepository.DeleteAsync(request.Id);

            return Result.SuccessFul();
        }
    }
}