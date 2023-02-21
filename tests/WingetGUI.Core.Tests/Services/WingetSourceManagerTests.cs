namespace WingetGUI.Core.Tests.Services
{
    public class WingetSourceManagerTests
    {
        private readonly CancellationToken cancellationToken = default;
        private Mock<IProcessManager> _processManagerMock;
        private IPackageSourceManager _sut;

        [SetUp]
        public void Setup()
        {
            _processManagerMock = new Mock<IProcessManager>();
            _sut = new WingetSourceManager(_processManagerMock.Object);
        }

        [Test]
        [TestCase("msstore https://storeedgefd.dsx.mp.microsoft.com/v9.0\nwinget  https://cdn.winget.microsoft.com/cache\n--------------\nName Argument", 2)]
        public async Task ShouldReturnExpectedNumberOfSources(string sourceData, int expectedNumberOfSources)
        {
            // Arrange
            _processManagerMock.Setup(m => m.ExecuteAsync(Constants.WingetProcessName, "source list", cancellationToken)).ReturnsAsync(new ProcessOutput { ExitCode = 0, Output = sourceData.Split("\n") });

            // Act
            var sources = await _sut.FetchInstalledSourcesAsync(cancellationToken);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(sources, Has.Count.EqualTo(expectedNumberOfSources));
                Assert.That(sources.All(source => source.HasValue), Is.True);
            });
            _processManagerMock.Verify(m => m.ExecuteAsync(Constants.WingetProcessName, "source list", cancellationToken), Times.Once);
        }

        [Test]
        [TestCase("0", true)]
        [TestCase("1", false)]
        public async Task ShouldReturnExpectedResultWhenRegisteringNewPackageSource(string exitCode, bool expectedResult)
        {
            // Arrange
            var source = new PackageSource { Name = "winget", Url = "https://cdn.winget.microsoft.com/cache" };
            var arguments = $"source add -n {source.Name} -a {source.Url} --accept-source-agreements";
            _processManagerMock.Setup(m => m.ExecuteAsync(It.IsAny<string>(), arguments, cancellationToken)).ReturnsAsync(new ProcessOutput { ExitCode = Convert.ToInt32(exitCode) });

            // Act
            var registerResult = await _sut.RegisterSourceAsync(source, cancellationToken);

            // Assert
            Assert.That(registerResult, Is.EqualTo(expectedResult));
            _processManagerMock.Verify(m => m.ExecuteAsync(It.IsAny<string>(), arguments, cancellationToken), Times.Once);
        }

        [Test]
        [TestCase("0", true)]
        [TestCase("1", false)]
        public async Task ShouldReturnExpectedResultWhenRegisteringNewPackageSourceWithType(string exitCode, bool expectedResult)
        {
            // Arrange
            var source = new PackageSource { Name = "winget", Url = "https://cdn.winget.microsoft.com/cache", Type = "msstore" };
            var arguments = $"source add -n {source.Name} -a {source.Url} -t {source.Type} --accept-source-agreements";
            _processManagerMock.Setup(m => m.ExecuteAsync(It.IsAny<string>(), arguments, cancellationToken)).ReturnsAsync(new ProcessOutput { ExitCode = Convert.ToInt32(exitCode) });

            // Act
            var registerResult = await _sut.RegisterSourceAsync(source, cancellationToken);

            // Assert
            Assert.That(registerResult, Is.EqualTo(expectedResult));
            _processManagerMock.Verify(m => m.ExecuteAsync(It.IsAny<string>(), arguments, cancellationToken), Times.Once);
        }

        [Test]
        public async Task ShouldReturnExpectedResultWhenRegisteringNewPackageSourceWithEmptySource()
        {
            // Arrange
            var source = new PackageSource();

            // Act
            var registerResult = await _sut.RegisterSourceAsync(source, cancellationToken);

            // Assert
            _processManagerMock.Verify(m => m.ExecuteAsync(It.IsAny<string>(), It.IsAny<string>(), cancellationToken), Times.Never);
        }

        [Test]
        [TestCase("0", true)]
        [TestCase("1", false)]
        public async Task ShouldReturnExpectedResultWhenUpdatingPackageSources(string exitCode, bool expectedResult)
        {
            // Arrange
            _processManagerMock.Setup(m => m.ExecuteAsync(It.IsAny<string>(), "source update", cancellationToken)).ReturnsAsync(new ProcessOutput { ExitCode = Convert.ToInt32(exitCode) });

            // Act
            var registerResult = await _sut.UpdateSourcesAsync(cancellationToken);

            // Assert
            Assert.That(registerResult, Is.EqualTo(expectedResult));
            _processManagerMock.Verify(m => m.ExecuteAsync(It.IsAny<string>(), "source update", cancellationToken), Times.Once);
        }

        [Test]
        [TestCase("0", true)]
        [TestCase("1", false)]
        public async Task ShouldReturnExpectedResultWhenResetingPackageSources(string exitCode, bool expectedResult)
        {
            // Arrange
            _processManagerMock.Setup(m => m.ExecuteAsync(It.IsAny<string>(), "source reset --force", cancellationToken)).ReturnsAsync(new ProcessOutput { ExitCode = Convert.ToInt32(exitCode) });

            // Act
            var registerResult = await _sut.ResetSourcesAsync(cancellationToken);

            // Assert
            Assert.That(registerResult, Is.EqualTo(expectedResult));
            _processManagerMock.Verify(m => m.ExecuteAsync(It.IsAny<string>(), "source reset --force", cancellationToken), Times.Once);
        }

        [Test]
        [TestCase("0", true)]
        [TestCase("1", false)]
        public async Task ShouldReturnExpectedResultWhenRemovingAPackageSource(string exitCode, bool expectedResult)
        {
            // Arrange
            var source = "msstore";
            var argument = $"source remove -n {source}";
            _processManagerMock.Setup(m => m.ExecuteAsync(It.IsAny<string>(), argument, cancellationToken)).ReturnsAsync(new ProcessOutput { ExitCode = Convert.ToInt32(exitCode) });

            // Act
            var registerResult = await _sut.RemoveSourceAsync(source, cancellationToken);

            // Assert
            Assert.That(registerResult, Is.EqualTo(expectedResult));
            _processManagerMock.Verify(m => m.ExecuteAsync(It.IsAny<string>(), argument, cancellationToken), Times.Once);
        }
    }
}
