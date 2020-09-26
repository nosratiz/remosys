using FluentValidation;
using Remosys.Api.Core.Application.Activity.Command;
using Remosys.Api.Core.Application.Activity.Dto;

namespace Remosys.Api.Core.Validator.Activity
{
    public class CreateUserActivityDtoValidator : AbstractValidator<CreateUserActivityDto>
    {
        public CreateUserActivityDtoValidator()
        {
            RuleFor(dto => dto.ApplicationName).NotEmpty();

            RuleFor(dto => dto).Must(x => x.ToDate >= x.FromDate);
        }
    }

    public class CreateUserActivityCommandValidator : AbstractValidator<CreateUserActivityCommand>
    {
        public CreateUserActivityCommandValidator()
        {
            RuleForEach(dto => dto.UserActivities)
                .NotEmpty()
                .SetValidator(new CreateUserActivityDtoValidator());


        }
    }
}