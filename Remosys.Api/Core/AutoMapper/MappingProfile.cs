using System;
using AutoMapper;
using Remosys.Api.Core.Application.Activity.Dto;
using Remosys.Api.Core.Application.Auth.Command.RegisterCommand;
using Remosys.Api.Core.Application.Contracts.Command.CreateContract;
using Remosys.Api.Core.Application.Contracts.Command.UpdateContract;
using Remosys.Api.Core.Application.Contracts.Dto;
using Remosys.Api.Core.Application.Departments.Command.Create;
using Remosys.Api.Core.Application.Departments.Command.Update;
using Remosys.Api.Core.Application.Departments.Dto;
using Remosys.Api.Core.Application.Employees.Command.Create;
using Remosys.Api.Core.Application.Employees.Command.Update;
using Remosys.Api.Core.Application.Employees.Dto;
using Remosys.Api.Core.Application.Files.Dto;
using Remosys.Api.Core.Application.Organization.Command.Create;
using Remosys.Api.Core.Application.Organization.Command.Update;
using Remosys.Api.Core.Application.Organization.Dto;
using Remosys.Api.Core.Application.Plans.Command.CreatePlan;
using Remosys.Api.Core.Application.Plans.Command.UpdatePlan;
using Remosys.Api.Core.Application.Plans.Dto;
using Remosys.Api.Core.Application.Roles.Dto;
using Remosys.Api.Core.Application.ToolCategories.Command.Create;
using Remosys.Api.Core.Application.ToolCategories.Command.Update;
using Remosys.Api.Core.Application.ToolCategories.Dto;
using Remosys.Api.Core.Application.Tools.Command.Create;
using Remosys.Api.Core.Application.Tools.Command.Update;
using Remosys.Api.Core.Application.Tools.Dto;
using Remosys.Api.Core.Application.Users.Command.CreateUser;
using Remosys.Api.Core.Application.Users.Command.UpdateProfile;
using Remosys.Api.Core.Application.Users.Command.UpdateUser;
using Remosys.Api.Core.Application.Users.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper;
using Remosys.Common.Types;
using UserDto = Remosys.Api.Core.Application.Users.Dto.UserDto;

namespace Remosys.Api.Core.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region User

            CreateMap<User, UserDto>();

            CreateMap<CreateUserCommand, User>()
                .ForMember(src => src.IsMobileConfirm, opt => opt.MapFrom(des => string.IsNullOrWhiteSpace(des.Mobile)))
                .ForMember(src => src.IsEmailConfirm, opt => opt.MapFrom(des => true))
                .ForMember(src => src.Password, opt => opt.MapFrom(des => PasswordManagement.HashPass(des.Password)))
                .ForMember(src => src.ActiveCode, opt => opt.MapFrom(des => new Random().Next(10000, 99999).ToString()))
                .ForMember(src => src.ModifiedDate, opt => opt.MapFrom(des => DateTime.Now))
                .ForMember(src => src.RegisterDate, opt => opt.MapFrom(des => DateTime.Now));

            CreateMap<UpdateUserCommand, User>()
                .ForMember(src => src.IsMobileConfirm, opt => opt.MapFrom(des => string.IsNullOrWhiteSpace(des.Mobile)))
                .ForMember(src => src.ModifiedDate, opt => opt.MapFrom(des => DateTime.Now));

            CreateMap<PagedResult<User>, PagedResult<UserDto>>();


            CreateMap<RegisterCommand, User>()
                .ForMember(x => x.ExpiredCode, opt => opt.MapFrom(des => DateTime.Now.AddMinutes(2)))
                .ForMember(src => src.ActiveCode, opt => opt.MapFrom(des => new Random().Next(10000, 99999).ToString()))
                .ForMember(x => x.RegisterDate, opt => opt.MapFrom(des => DateTime.Now))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(des => DateTime.Now));

            CreateMap<CreateEmployeeCommand, User>()
                .ForMember(x => x.ExpiredCode, opt => opt.MapFrom(des => DateTime.Now.AddMinutes(2)))
                .ForMember(src => src.ActiveCode, opt => opt.MapFrom(des => new Random().Next(10000, 99999).ToString()))
                .ForMember(x => x.RegisterDate, opt => opt.MapFrom(des => DateTime.Now))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(des => DateTime.Now));
            ;

            CreateMap<UpdateProfileCommand, User>();
            #endregion

            #region Role
            CreateMap<Role, UserRoleDto>();


            CreateMap<Role, RoleDto>();
            #endregion


            #region Plan

            CreateMap<PagedResult<Plan>, PagedResult<PlanDto>>();

            CreateMap<Plan, PlanDto>();

            CreateMap<CreatePlanCommand, Plan>();

            CreateMap<UpdatePlanCommand, Plan>();

            #endregion


            #region Contract

            CreateMap<PagedResult<Contract>, PagedResult<ContractDto>>();

            CreateMap<Contract, ContractDto>().ForMember(src => src.FullName, opt => opt.MapFrom(des => $"{des.User.FirstName} {des.User.LastName}"))
                .ForMember(src => src.Email, opt => opt.MapFrom(des => des.User.Email))
                .ForMember(src => src.Mobile, opt => opt.MapFrom(des => des.User.Mobile));

            CreateMap<CreateContractCommand, Contract>().ForMember(x => x.CreateDate, opt => opt.MapFrom(des => DateTime.Now));

            CreateMap<UpdateContractCommand, Contract>();

            #endregion

            CreateMap<CreateUserActivityDto, UserActivity>();

            CreateMap<UserFile, FileDto>();

            #region Employee

            CreateMap<PagedResult<Employee>, PagedResult<EmployeeDto>>();

            CreateMap<Employee, EmployeeDto>().ForMember(x => x.Mobile, opt => opt.MapFrom(des => des.User.Mobile))
                .ForMember(x => x.Email, opt => opt.MapFrom(des => des.User.Email))
                .ForMember(x => x.FirstName, opt => opt.MapFrom(des => des.User.FirstName))
                .ForMember(x => x.LastName, opt => opt.MapFrom(des => des.User.LastName))
                .ForMember(x => x.UserId, opt => opt.MapFrom(des => des.User.Id));

            CreateMap<CreateEmployeeCommand, Employee>().ForMember(x => x.CreateDate, opt => opt.MapFrom(des => DateTime.Now));

            CreateMap<UpdateEmployeeCommand, Employee>();

            #endregion

            #region Tools

            CreateMap<PagedResult<Tool>, PagedResult<ToolDto>>();

            CreateMap<Tool, ToolDto>();

            CreateMap<CreateToolCommand, Tool>();

            CreateMap<UpdateToolCommand, Tool>();

            #endregion

            #region ToolCategory

            CreateMap<PagedResult<ToolsCategory>, PagedResult<ToolCategoryDto>>();

            CreateMap<ToolsCategory, ToolCategoryDto>();

            CreateMap<CreateCategoryCommand, ToolsCategory>().ForMember(x => x.CreateDate, opt => opt.MapFrom(des => DateTime.Now));

            CreateMap<UpdateCategoryCommand, ToolsCategory>();

            #endregion

            #region Organization

            CreateMap<PagedResult<Models.Organization>, PagedResult<OrganizationDto>>();

            CreateMap<Models.Organization, OrganizationDto>();

            CreateMap<CreateOrganizationCommand, Models.Organization>()
                .ForMember(x => x.CreateDate, opt => opt.MapFrom(des => DateTime.Now));

            CreateMap<UpdateOrganizationCommand, Models.Organization>();

            #endregion

            #region Department

            CreateMap<PagedResult<Department>, PagedResult<DepartmentDto>>();

            CreateMap<Department, DepartmentDto>();

            CreateMap<CreateDepartmentCommand, Department>().ForMember(x => x.CreateDate, opt => opt.MapFrom(des => DateTime.Now));

            CreateMap<UpdateDepartmentCommand, Department>();

            #endregion

        }
    }
}