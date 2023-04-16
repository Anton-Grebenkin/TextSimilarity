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

namespace TextSimilarity.API.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        //internal static void AddApplicationModules(this IServiceCollection services)
        //{
        //    var modules = typeof(IApplicationModule).Assembly
        //        .GetTypes()
        //        .Where(p => p.IsClass && p.IsAssignableTo(typeof(IApplicationModule)))
        //        .Select(Activator.CreateInstance)
        //        .Cast<IApplicationModule>();

        //    foreach (var module in modules)
        //    {
        //        module.AddModule(services);
        //    }
        //}

        internal static void AddMappingConfiguration(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
        }

        internal static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRevokeAPIKeyRepository, RevokeAPIKeyRepository>();
            services.AddScoped<IGenerateAPIKeyRepository, GenerateAPIKeyRepository>();
            services.AddScoped<IAPIKeyRepository, APIKeyRepository>();
            services.AddScoped<IAPIKeyService, APIKeyService>();
            services.AddScoped<IJWTService, JWTService>();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IRegisterRepository, RegisterRepository>();
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
