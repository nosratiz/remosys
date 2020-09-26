using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Remosys.Common.Redis
{
    public static class Extensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RedisOptions>(configuration.GetSection("RedisOptions"));

            var redisOptions = new RedisOptions();
            configuration.Bind(nameof(RedisOptions), redisOptions);

            services.AddSingleton(redisOptions);

            services.AddDistributedRedisCache(o =>
            {
                o.Configuration = redisOptions.ConnectionString;
                o.InstanceName = redisOptions.Instance;
            });

            return services;
        }
    }
}