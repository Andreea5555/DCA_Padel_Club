using DCA_Padel_Club.Core.Tools.ObjectMapper;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests.ObjectMapperTests;

public class SpecificMappingTests
{
    public record NamePair(string First, string Last);
    public record FullName(string Value);

    public class FullNameConfig : IMappingConfig<NamePair, FullName>
    {
        public FullName Map(NamePair input) => new($"{input.First} {input.Last}");
    }

    [Fact]
    public void Map_Uses_Registered_IMappingConfig_When_Available()
    {
        ServiceCollection services = new();
        services.AddSingleton<IMappingConfig<NamePair, FullName>, FullNameConfig>();
        ServiceProvider provider = services.BuildServiceProvider();
        JsonObjectMapper mapper = new(provider);

        FullName result = mapper.Map<FullName>(new NamePair("Ada", "Lovelace"));

        Assert.Equal("Ada Lovelace", result.Value);
    }

    [Fact]
    public void Map_Falls_Back_To_Json_When_No_Config_Registered()
    {
        ServiceProvider provider = new ServiceCollection().BuildServiceProvider();
        JsonObjectMapper mapper = new(provider);

        FullName result = mapper.Map<FullName>(new FullName("Already-Joined"));

        Assert.Equal("Already-Joined", result.Value);
    }
}
