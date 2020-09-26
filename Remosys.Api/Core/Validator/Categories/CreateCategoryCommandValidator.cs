using FluentValidation;
using Remosys.Api.Core.Application.ToolCategories.Command.Create;
using Remosys.Common.Helper.systemMessage;

namespace Remosys.Api.Core.Validator.Categories
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(dto => dto.Name).NotEmpty().NotNull().WithMessage(ResponseMessage.NameIsRequired);
        }
    }
}