using WingetGUI.Core.Tests.Utils;

namespace WingetGUI.Core.Tests.Services
{
    public class WingetFeaturesServiceTests
    {
        private readonly CancellationToken cancellationToken = default;
        private Mock<IProcessManager> _processManagerMock;
        private IFeaturesService _sut;

        [SetUp]
        public void Setup()
        {
            _processManagerMock = new Mock<IProcessManager>();
            _sut = new WingetFeaturesService(_processManagerMock.Object);
        }

        [Test]
        public async Task ShouldReturnExpectedFeatures()
        {
            // Arrange
            var outputLines = new[]
            {
                "Zip Installation              Disabled zipInstall   https://aka.ms/winget-settings",
                "Direct MSI Installation       Disabled directMSI    https://aka.ms/winget-settings",
                "Show Dependencies Information Disabled dependencies https://aka.ms/winget-settings",
              "----------------------------------------------------------------------------------",
                "Feature                       Status   Property     Link",
                "They can be configured through the settings file 'winget settings'.",
                "The following experimental features are in progress.",
            }.Reverse().ToList();
            var processOutput = new ProcessOutput { Output = outputLines };
            var expectedFeatures = new List<Feature>
            {
                new Feature
                {
                    Name= "Show Dependencies Information",
                    Enabled = false,
                    Property = "dependencies",
                    Link = "https://aka.ms/winget-settings"
                },
                new Feature
                {
                    Name= "Direct MSI Installation",
                    Enabled = false,
                    Property = "directMSI",
                    Link = "https://aka.ms/winget-settings"
                },
                new Feature
                {
                    Name= "Zip Installation",
                    Enabled = false,
                    Property = "zipInstall",
                    Link = "https://aka.ms/winget-settings"
                },
            };
            _processManagerMock.Setup(m => m.ExecuteAsync(Constants.WingetProcessName, "features", cancellationToken)).ReturnsAsync(processOutput);

            // Act
            var features = await _sut.FetchFeaturesAsync(cancellationToken);

            // Assert
            Assert.That(CollectionMatcher.Match(expectedFeatures, features), Is.True);
            _processManagerMock.Verify(m => m.ExecuteAsync(Constants.WingetProcessName, "features", cancellationToken), Times.Once);
        }
    }
}
