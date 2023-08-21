using System.Security.Cryptography;
using AutoMapper;

namespace Microservices.Services.Order.Application.Mapping;

public static class ObjectMapper
{
    //Getting IMapper without dependency injection
    private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfiles>();
        });

        return config.CreateMapper();
    });

    public static IMapper Mapper => lazy.Value;
}