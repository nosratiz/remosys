using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Remosys.Common.Types;

namespace Remosys.Common.Mongo
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbOptions>(configuration.GetSection("MongoDbOptions"));

            var mongoDbOptions = new MongoDbOptions();
            configuration.Bind(nameof(MongoDbOptions), mongoDbOptions);

            services.AddSingleton(mongoDbOptions);

            var mongoClient = new MongoClient(mongoDbOptions.ConnectionString);

            services.AddSingleton(mongoClient);

            services.AddScoped(provider => mongoClient.GetDatabase(mongoDbOptions.Database));

            return services;

        }


        public static void AddMongoRepository<TEntity>(this IServiceCollection services, string collectionName)
            where TEntity : IIdentifiable
            => services.AddScoped<IMongoRepository<TEntity>>(provider =>
                new MongoRepository<TEntity>(provider.GetService<IMongoDatabase>(), collectionName));
    }


}
