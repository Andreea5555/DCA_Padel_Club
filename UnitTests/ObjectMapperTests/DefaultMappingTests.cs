using DCA_Padel_Club.Core.Tools.ObjectMapper;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests.ObjectMapperTests;

public class DefaultMappingTests
{
    private record Input(string Name);
    private record Output(string Name);

    private record Inner(string Tag, int Count);
    private record Outer(string Title, Inner Inner);
    private record OuterMirror(string Title, Inner Inner);

    [Fact]
    public void Map_To_Record_With_Identical_Shape_Uses_Json_Fallback()
    {
        ServiceProvider provider = new ServiceCollection().BuildServiceProvider();
        JsonObjectMapper mapper = new(provider);

        Output result = mapper.Map<Output>(new Input("Alice"));

        Assert.Equal("Alice", result.Name);
    }

    [Fact]
    public void Map_Complex_Nested_Graph_Uses_Json_Fallback()
    {
        ServiceProvider provider = new ServiceCollection().BuildServiceProvider();
        JsonObjectMapper mapper = new(provider);

        Outer source = new("Schedule April", new Inner("Tag-1", 42));
        OuterMirror result = mapper.Map<OuterMirror>(source);

        Assert.Equal("Schedule April", result.Title);
        Assert.Equal("Tag-1", result.Inner.Tag);
        Assert.Equal(42, result.Inner.Count);
    }
}
