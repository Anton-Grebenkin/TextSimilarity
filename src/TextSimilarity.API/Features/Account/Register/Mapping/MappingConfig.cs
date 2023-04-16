using Mapster;
using TextSimilarity.API.Common.DataAccess.Entities;
using TextSimilarity.API.Features.Account.Register.UseCase;

namespace TextSimilarity.API.Features.Account.Register.Mapping
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RegisterRequest, User>();
        }
    }
}
