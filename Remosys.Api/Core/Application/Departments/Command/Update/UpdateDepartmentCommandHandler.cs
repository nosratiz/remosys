using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Departments.Command.Update
{
    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, Result>
    {
        private readonly IMongoRepository<Employee> _employeeRepository;
        private readonly IMongoRepository<Department> _departmentRepository;
        private readonly IMongoRepository<ToolsCategory> _toolCategoryRepository;
        private readonly IMapper _mapper;

        public UpdateDepartmentCommandHandler(IMongoRepository<Employee> employeeRepository, IMongoRepository<Department> departmentRepository, IMongoRepository<ToolsCategory> toolCategoryRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _toolCategoryRepository = toolCategoryRepository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.GetAsync(request.Id, cancellationToken);

            if (department is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.DepartmentNotFound)));

            _mapper.Map(request, department);

            #region Validation 

            if (request.ToolCategoryIds != null)
            {
                List<ToolsCategory> toolsCategories = new List<ToolsCategory>();

                foreach (var id in request.ToolCategoryIds)
                {
                    var category = await _toolCategoryRepository.GetAsync(x => x.IsDeleted == false && x.Id == id, cancellationToken);

                    if (category is null)
                        return Result.Failed(
                            new BadRequestObjectResult(new ApiMessage($"دسته بندی با ایدی {id} یافت نشد")));

                    toolsCategories.Add(category);
                }

                department.ToolsCategories = toolsCategories;
            }

            if (request.EmployeeIds != null)
            {
                List<Employee> employees = new List<Employee>();
                foreach (var id in request.EmployeeIds)
                {
                    var employee = await _employeeRepository.GetAsync(x => x.IsDeleted == false && x.Id == id, cancellationToken);

                    if (employee is null)
                        return Result.Failed(
                            new BadRequestObjectResult(new ApiMessage($"کارمندی با شماره ایدی {id} پیدا نشد")));

                    employees.Add(employee);
                }

                department.Employees = employees;
            }

            if (request.ManagerId.HasValue)
            {
                var manager =
                    await _employeeRepository.GetAsync(x => x.IsDeleted == false && x.Id == request.ManagerId.Value, cancellationToken);

                if (manager is null)
                    return Result.Failed(
                        new BadRequestObjectResult(new ApiMessage("مدیر با ایدی ارسالی پیدا نشد")));

                department.Manager = manager;
            }

            #endregion

            await _departmentRepository.UpdateAsync(department);

            return Result.SuccessFul();
        }
    }
}