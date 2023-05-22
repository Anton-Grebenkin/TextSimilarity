using Mapster;
using MapsterMapper;
using System.Reflection;
using FluentValidation;
using MediatR;
using TextSimilarity.API.Common.PipelineBehaviours;
using TextSimilarity.API.Features.Account.Register.Repository;
using TextSimilarity.API.Common.Security.Authentication;
using TextSimilarity.API.Common.Security.Authorization;
using TextSimilarity.API.Features.Account.Login.Repository;
using TextSimilarity.API.Features.Account.GenerateAPIKey.Repository;
using TextSimilarity.API.Features.Account.RevokeAPIKey.Repository;
using TextSimilarity.API.Features.Account.GetAPIKey.Repository;
using TextSimilarity.API.Features.Account.GetAPIHistory.Repository;
using TextSimilarity.API.Common.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace TextSimilarity.API.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {

        internal static void AddMappingConfiguration(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
        }

        internal static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JWTSettings>(configuration.GetSection(nameof(JWTSettings)));
            services.AddDbContext<AppDataContext>(c => c.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IAppDataContextProvider>(x => new AppDataContextProvider(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IRevokeAPIKeyRepository, RevokeAPIKeyRepository>();
            services.AddScoped<IGenerateAPIKeyRepository, GenerateAPIKeyRepository>();
            services.AddScoped<IAPIKeyRepository, APIKeyRepository>();
            services.AddScoped<IAPIKeyService, APIKeyService>();
            services.AddScoped<IJWTService, JWTService>();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IRegisterRepository, RegisterRepository>();
            services.AddScoped<IGetAPIKeyRepository, GetAPIKeyRepository>();
            services.AddScoped<IGetAPIHistoryRepository, GetAPIHistoryRepository>();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            });


            return services;
        }
    }
}
