using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Remosys.Api.Core.Application.Plans.Command.CreatePlan;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;

namespace Remosys.Api.Core.Validator.Plans
{
    public class CreatePlanCommandValidator : AbstractValidator<CreatePlanCommand>
    {
        private readonly IMongoRepository<Plan> _planRepository;
        public CreatePlanCommandValidator(IMongoRepository<Plan> planRepository)
        {
            _planRepository = planRepository;
            CascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.Name).NotEmpty().WithMessage(ResponseMessage.NameIsRequired);

            RuleFor(dto => dto.Price).NotEmpty().GreaterThanOrEqualTo(0);

            RuleFor(dto => dto.Month).NotEmpty().GreaterThan(0).LessThanOrEqualTo(12);

            RuleFor(dto => dto.PersonCount).NotEmpty().GreaterThan(0);

            RuleFor(dto => dto).MustAsync(ValidName).MustAsync(DuplicatePlan);
        }

        private async Task<bool> ValidName(CreatePlanCommand createPlanCommand, CancellationToken cancellationToken)
        {
            return !await _planRepository.ExistsAsync(x=>x.IsDeleted==false && x.Name==createPlanCommand.Name,cancellationToken);
        }

        private async Task<bool> DuplicatePlan(CreatePlanCommand createPlanCommand, CancellationToken cancellationToken)
        {
            return !await _planRepository.ExistsAsync(
                x => x.IsDeleted == false && x.PersonCount == createPlanCommand.PersonCount &&
                     x.Month == createPlanCommand.Month && x.Price == createPlanCommand.Price, cancellationToken);
        }


    }
}