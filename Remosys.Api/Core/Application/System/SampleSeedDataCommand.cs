using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Remosys.Api.Core.Models;
using Remosys.Common.Mongo;

namespace Remosys.Api.Core.Application.System
{
    public class SampleSeedDataCommand : IRequest
    {
    }

    public class SampleSeedDataCommandHandler :IRequestHandler<SampleSeedDataCommand>
    {
        private readonly IMongoRepository<User> _useRepository;
        private readonly IMongoRepository<Role> _roleRepository;

        public SampleSeedDataCommandHandler(IMongoRepository<User> useRepository, IMongoRepository<Role> roleRepository)
        {
            _useRepository = useRepository;
            _roleRepository = roleRepository;
        }

        public async Task<Unit> Handle(SampleSeedDataCommand request, CancellationToken cancellationToken)
        {
           var seeder=new SeedData(_useRepository,_roleRepository);

           await seeder.SeedAllAsync(cancellationToken);

           return Unit.Value;
        }
    }
}
