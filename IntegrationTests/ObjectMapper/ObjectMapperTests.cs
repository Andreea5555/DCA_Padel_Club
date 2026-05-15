using DCA_Padel_Club.Core.Tools.ObjectMapper;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTests.ObjectMapper;

public class ObjectMapperTests
{
    [Fact]
    public void Map_ObjectsWithSameStructure_ShouldUseDefaultJsonMapping()
    {
        IServiceProvider serviceProvider = new ServiceCollection()
            .BuildServiceProvider();

        IMapper mapper = new JsonObjectMapper(serviceProvider);

        TestInput input = new("John", "Doe");

        TestOutput output = mapper.Map<TestOutput>(input);

        Assert.Equal("John", output.FirstName);
        Assert.Equal("Doe", output.LastName);
    }

    [Fact]
    public void Map_WithSpecificMappingConfig_ShouldUseMappingConfig()
    {
        IServiceProvider serviceProvider = new ServiceCollection()
            .AddScoped<IMappingConfig<NameInput, NameOutput>, NameMappingConfig>()
            .BuildServiceProvider();

        IMapper mapper = new JsonObjectMapper(serviceProvider);

        NameInput input = new("John", "Doe");

        NameOutput output = mapper.Map<NameOutput>(input);

        Assert.Equal("John Doe", output.FullName);
    }

    [Fact]
    public void Map_ComplexObject_ShouldMapNestedObject()
    {
        IServiceProvider serviceProvider = new ServiceCollection()
            .BuildServiceProvider();

        IMapper mapper = new JsonObjectMapper(serviceProvider);

        ComplexInput input = new(
            "Schedule 1",
            new CourtInput("Court 1"));

        ComplexOutput output = mapper.Map<ComplexOutput>(input);

        Assert.Equal("Schedule 1", output.Name);
        Assert.Equal("Court 1", output.Court.CourtId);
    }

    private record TestInput(string FirstName, string LastName);

    private record TestOutput(string FirstName, string LastName);

    public record NameInput(string FirstName, string LastName);

    public record NameOutput(string FullName);

    public class NameMappingConfig : IMappingConfig<NameInput, NameOutput>
    {
        public NameOutput Map(NameInput input)
        {
            return new NameOutput($"{input.FirstName} {input.LastName}");
        }
    }

    private record ComplexInput(string Name, CourtInput Court);

    private record CourtInput(string CourtId);

    private record ComplexOutput(string Name, CourtOutput Court);

    private record CourtOutput(string CourtId);
}