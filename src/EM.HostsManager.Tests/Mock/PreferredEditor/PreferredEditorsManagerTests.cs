using EM.HostsManager.PreferredEditor;
using EM.HostsManager.Settings;

namespace EM.HostsManager.Tests.Mock.PreferredEditor
{
    public class PreferredEditorsManagerTests
    {
        private static Mock<IEditor> GetMockPreferredEditor()
        {
            var mockEditor = new Mock<IEditor>();

            mockEditor.SetupGet(p => p.DisplayName).Returns("Mock Editor");
            mockEditor.SetupGet(p => p.IsDefault).Returns(true);
            mockEditor.SetupGet(p => p.IsSelected).Returns(true);
            mockEditor.SetupGet(p => p.Key).Returns("MockPreferredEditor");
            mockEditor.Setup(e => e.Open()).Returns(true);

            return mockEditor;
        }

        [Fact]
        [Trait("Category", "Mock")]
        public void PreferredEditor_Works()
        {
            // Arrange
            var settingsProvider = new Mock<ISettingsProvider>();

            settingsProvider.Setup(sp => sp.GetValue(It.IsAny<string>(), It.IsAny<string>())).Returns("MockPreferredEditor");
            settingsProvider.Setup(sp => sp.SetValue(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var mockEditor = GetMockPreferredEditor();
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
    }
}
