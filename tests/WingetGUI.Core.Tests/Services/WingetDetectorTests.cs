namespace WingetGUI.Core.Tests.Services
{
    public class WingetDetectorTests
    {
        private readonly CancellationToken cancellationToken = default;
        private Mock<IProcessManager> _processManagerMock;
        private IWingetDetector _sut;

        [SetUp]
        public void Setup()
        {
            _processManagerMock = new Mock<IProcessManager>();
            _sut = new WingetDetector(_processManagerMock.Object);
        }

        [Test]
        [TestCase("0", true)]
        [TestCase("1", false)]
        public async Task ShouldReturnExpectedResultWhenDetectingWingetInstallation(string exitCode, bool expectedResult)
        {
            // Arrange
            _processManagerMock.Setup(m => m.ExecuteAsync(It.IsAny<string>(), "", cancellationToken)).ReturnsAsync(new Models.ProcessOutput { ExitCode = Convert.ToInt32(exitCode) });

            // Act
            var isDetected = await _sut.DetectAsync(cancellationToken);

            // Assert
            Assert.That(isDetected, Is.EqualTo(expectedResult));
            _processManagerMock.Verify(m => m.ExecuteAsync(It.IsAny<string>(), "", cancellationToken), Times.Once);
        }

        [Test]
        public async Task ShouldReturnExpectedResultWhenDetectingWingetInstallationReturnsNull()
        {
            // Act
            var isDetected = await _sut.DetectAsync(cancellationToken);

            // Assert
            Assert.That(isDetected, Is.False);
            _processManagerMock.Verify(m => m.ExecuteAsync(It.IsAny<string>(), "", cancellationToken), Times.Once);
        }

        [Test]
        public async Task ShouldReturnExpectedVersionWhileWueryingForIt()
        {
            // Arrange
            var version = "v1.4.2161-preview";
            _processManagerMock.Setup(m => m.ExecuteAsync(It.IsAny<string>(), "--version", cancellationToken)).ReturnsAsync(new ProcessOutput { Output = new List<string> { version } });

            // Act
            var response = await _sut.FetchInstalledVersionAsync(cancellationToken);

            // Assert
            Assert.That(version, Is.EqualTo(response));
            _processManagerMock.Verify(m => m.ExecuteAsync(It.IsAny<string>(), "--version", cancellationToken), Times.Once);
        }
    }
}
