using Microscope.Domain.Aggregates.RemoteConfig;
using Microscope.Domain.Aggregates.RemoteConfigAggregate;
using Microscope.Infrastructure.Repositories.Base;

namespace Microscope.Infrastructure.Repositories
{
    public class RemoteConfigRepository : EFRepositoryBase<RemoteConfig>, IRemoteConfigRepository
    {
        public RemoteConfigRepository(MicroscopeDbContext context) : base(context)
        {
            
        }
    }
}
