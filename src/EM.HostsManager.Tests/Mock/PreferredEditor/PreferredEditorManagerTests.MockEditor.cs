using EM.HostsManager.PreferredEditor;

namespace EM.HostsManager.Tests.Mock.PreferredEditor;

public partial class PreferredEditorManagerTests
{
    private static Mock<IEditor> BuildMockEditor(string displayName, string key, bool isDefault, bool isSelected, bool openResult)
    {
        var mockEditor = new Mock<IEditor>();

        mockEditor.SetupGet(p => p.DisplayName).Returns(displayName);
        mockEditor.SetupGet(p => p.Key).Returns(key);
        mockEditor.SetupGet(p => p.IsDefault).Returns(isDefault);
        mockEditor.SetupGet(p => p.IsSelected).Returns(isSelected);
        mockEditor.Setup(e => e.Open()).Returns(openResult);
        
        return mockEditor;
    }
}
