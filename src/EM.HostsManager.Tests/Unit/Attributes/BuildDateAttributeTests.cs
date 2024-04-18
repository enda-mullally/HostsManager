namespace EM.HostsManager.Tests.Unit.Attributes;

[Trait("Category", "Unit")]
public class BuildDateAttributeTests
{
    [Fact]
    public void BuildDateAttribute_Properties_Work()
    {
        const string buildDateString = "test build date";

        var sut = new BuildDateAttribute(buildDateString);

        sut.BuildDate.Should().BeEquivalentTo(buildDateString);
    }
}
