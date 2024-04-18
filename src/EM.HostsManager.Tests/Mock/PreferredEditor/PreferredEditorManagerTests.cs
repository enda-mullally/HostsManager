using EM.HostsManager.PreferredEditor;
using EM.HostsManager.Settings;

namespace EM.HostsManager.Tests.Mock.PreferredEditor;

public partial class PreferredEditorManagerTests
{
    [Fact]
    [Trait("Category", "Mock")]
    public void PreferredEditor_Works()
    {
        // Arrange
        var settingsProvider = new Mock<ISettingsProvider>();

        settingsProvider.Setup(sp => sp.GetValue(It.IsAny<string>(), It.IsAny<string>()))
            .Returns("MockPreferredEditorKey");
        settingsProvider.Setup(sp => sp.SetValue(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        var mockEditor = BuildMockEditor("Mock Editor", "MockPreferredEditorKey", true, true, true);
        var sut = new PreferredEditorManager(settingsProvider.Object).RegisterEditors([mockEditor.Object]);

        // Act
        sut.GetEditors();
        sut.SaveSelectedEditor("Any");
        var openResult = sut.Open();

        // Assert
        openResult.Should().BeTrue();
        settingsProvider.Verify(sp => sp.GetValue(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
        settingsProvider.Verify(sp => sp.SetValue(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
        mockEditor.Verify(me => me.Open(), Times.AtLeastOnce);
    }

    [Fact]
    [Trait("Category", "Mock")]
    public void PreferredEditor_Selected_Editor_Fails_To_Open()
    {
        // Arrange
        var settingsProvider = new Mock<ISettingsProvider>();

        settingsProvider.Setup(sp => sp.GetValue(It.IsAny<string>(), It.IsAny<string>()))
            .Returns("DefaultKey");
        settingsProvider.Setup(sp => sp.SetValue(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        var defaultMockEditor = BuildMockEditor("Default Mock Editor", "DefaultKey", true, true, false);
        
        var sut =
            new PreferredEditorManager(settingsProvider.Object).
                RegisterEditors([defaultMockEditor.Object]);

        // Act
        var openResult = sut.Open();

        // Assert
        openResult.Should().BeFalse();
        settingsProvider.Verify(sp => sp.GetValue(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
        defaultMockEditor.Verify(me => me.Open(), Times.AtLeastOnce);
    }

    [Fact]
    [Trait("Category", "Mock")]
    public void PreferredEditor_No_Selected_Editor_Skips_Open()
    {
        // Arrange
        var settingsProvider = new Mock<ISettingsProvider>();

        settingsProvider.Setup(sp => sp.GetValue(It.IsAny<string>(), It.IsAny<string>()))
            .Returns("DefaultKey");
        settingsProvider.Setup(sp => sp.SetValue(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        var defaultMockEditor = BuildMockEditor("Default Mock Editor, but Not Selected, it will never open", "DefaultKey", true, false, true);

        var sut =
            new PreferredEditorManager(settingsProvider.Object).
                RegisterEditors([defaultMockEditor.Object]);

        // Act
        var openResult = sut.Open();

        // Assert
        openResult.Should().BeFalse();
        settingsProvider.Verify(sp => sp.GetValue(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
        defaultMockEditor.Verify(me => me.Open(), Times.Never);
    }
}
