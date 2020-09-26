using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Remosys.Api.Core.Application.Plans.Command.UpdatePlan;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;

namespace Remosys.Api.Core.Validator.Plans
{
    public class UpdatePlanCommandValidator : AbstractValidator<UpdatePlanCommand>
    {
        private readonly IMongoRepository<Plan> _planRepository;
        public UpdatePlanCommandValidator(IMongoRepository<Plan> planRepository)
        {
            _planRepository = planRepository;
            CascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.Name).NotEmpty().WithMessage(ResponseMessage.NameIsRequired);

            RuleFor(dto => dto.Price).NotEmpty().GreaterThanOrEqualTo(0);

            RuleFor(dto => dto.Month).NotEmpty().GreaterThan(0).LessThanOrEqualTo(12);

            RuleFor(dto => dto.PersonCount).NotEmpty().GreaterThan(0);

            RuleFor(dto => dto).MustAsync(ValidName).MustAsync(DuplicatePlan);
        }

        private async Task<bool> ValidName(UpdatePlanCommand updatePlanCommand, CancellationToken cancellationToken)
        {
            return !await _planRepository.ExistsAsync(x => x.IsDeleted == false && x.Name == updatePlanCommand.Name, cancellationToken);
        }

        private async Task<bool> DuplicatePlan(UpdatePlanCommand updatePlanCommand, CancellationToken cancellationToken)
        {
            return !await _planRepository.ExistsAsync(
                x => x.IsDeleted == false && x.Id != updatePlanCommand.Id && x.PersonCount == updatePlanCommand.PersonCount &&
                     x.Month == updatePlanCommand.Month && x.Price == updatePlanCommand.Price, cancellationToken);
        }
    }
}