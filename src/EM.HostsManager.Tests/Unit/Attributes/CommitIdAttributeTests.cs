namespace EM.HostsManager.Tests.Unit.Attributes;

[Trait("Category", "Unit")]
public class CommitIdAttributeTests
{
    [Fact]
    public void CommitIdAttribute_Properties_Work()
    {
        const string commitId = "commit id test";

        var sut = new CommitIdAttribute(commitId);

        sut.CommitId.Should().BeEquivalentTo(commitId);
    }
}
