using WingetGUI.Core.Tests.Utils;

namespace WingetGUI.Core.Tests.Services
{
    public class WingetPackageManagerTests
    {
        private IList<string> data, errorData;
        private readonly CancellationToken cancellationToken = default;
        private Mock<IProcessManager> _processManagerMock;
        private IPackageManager _sut;

        [SetUp]
        public void Setup()
        {
            data = new List<string>();
            errorData = new List<string>();
            _processManagerMock = new Mock<IProcessManager>();
            _sut = new WingetPackageManager(_processManagerMock.Object);
        }

        [Test]
        [TestCase("Cactus Blockchain                             CactusNetwork.CactusBlockchainGUI 1.3.4            Tag: full-node winget\nChia Blockchain                               ChiaNetwork.GUIforChiaBlockchain  1.6.0            Tag: full-node winget\n-----------------------------------------------------------------------------------------------------------------------\nName                                          Id                                Version          Match          Source\n   \b-\b\\\b|\b/\b-\b\\\b|\b ", 2)]
        public async Task ShouldReturnExpectedNumberOfPackages(string sourceData, int expectedNumberOfPackages)
        {
            // Arrange
            var searchTerm = "node";
            _processManagerMock.Setup(m => m.ExecuteAsync(Constants.WingetProcessName, $"search {searchTerm} --accept-source-agreements", cancellationToken)).ReturnsAsync(new ProcessOutput { ExitCode = 0, Output = sourceData.Split("\n") });

            // Act
            var packages = await _sut.SearchPackages(searchTerm, cancellationToken);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(packages, Has.Count.EqualTo(expectedNumberOfPackages));
                Assert.That(packages.All(source => source.HasValue), Is.True);
            });
            _processManagerMock.Verify(m => m.ExecuteAsync(Constants.WingetProcessName, $"search {searchTerm} --accept-source-agreements", cancellationToken), Times.Once);
        }

        [Test]
        [TestCase("0", true)]
        [TestCase("1", false)]
        public async Task ShouldReturnExpectedResultWhenInstallingAPackage(string exitCode, bool expectedResult)
        {
            // Act
            await _sut.InstallAsync("node", OnDataReceived, OnErrorRecevied, cancellationToken);

            // Assert
            _processManagerMock.Verify(m => m.StreamAsync(It.IsAny<string>(), "install node", OnDataReceived, OnErrorRecevied, cancellationToken), Times.Once);
        }

        [Test]
        [TestCase("0", true)]
        [TestCase("1", false)]
        public async Task ShouldReturnExpectedResultWhenUninstallingAPackage(string exitCode, bool expectedResult)
        {
            // Act
            await _sut.UninstallAsync("node", OnDataReceived, OnErrorRecevied, cancellationToken);

            // Assert
            _processManagerMock.Verify(m => m.StreamAsync(It.IsAny<string>(), "uninstall node", OnDataReceived, OnErrorRecevied, cancellationToken), Times.Once);
        }

        [Test]
        [TestCase("12 upgrades available.\nDiscord                                             Discord.Discord                1.0.9005         1.0.9007     winget\nAndroid Studio                                      Google.AndroidStudio.Canary    2022.2.1.1       2022.2.1.6   winget\n-----------------------------------------------------------------------------------------------------------------------\nName                                                Id                             Version          Available    Source\n   \b-\b \n   \b-\b\\\b|\b/\b ", 2)]
        public async Task ShouldFetchUpgradeablePackages(string sourceData, int expectedNumberOfPackages)
        {
            // Arrange
            _processManagerMock.Setup(m => m.ExecuteAsync(Constants.WingetProcessName, "upgrade", cancellationToken)).ReturnsAsync(new ProcessOutput { ExitCode = 0, Output = sourceData.Split("\n") });

            // Act
            var packages = await _sut.FetchUpdgradablePackages(false, cancellationToken);

            // Assert
            Assert.That(packages, Has.Count.EqualTo(expectedNumberOfPackages));
            _processManagerMock.Verify(m => m.ExecuteAsync(Constants.WingetProcessName, "upgrade", cancellationToken), Times.Once);
        }

        [Test]
        [TestCase("12 upgrades available.\nDiscord                                             Discord.Discord                1.0.9005         1.0.9007     winget\nAndroid Studio                                      Google.AndroidStudio.Canary    2022.2.1.1       2022.2.1.6   winget\n-----------------------------------------------------------------------------------------------------------------------\nName                                                Id                             Version          Available    Source\n   \b-\b \n   \b-\b\\\b|\b/\b ", 2)]
        public async Task ShouldFetchUpgradeablePackagesWithIncludeUnknown(string sourceData, int expectedNumberOfPackages)
        {
            // Arrange
            _processManagerMock.Setup(m => m.ExecuteAsync(Constants.WingetProcessName, "upgrade --include-unknown", cancellationToken)).ReturnsAsync(new ProcessOutput { ExitCode = 0, Output = sourceData.Split("\n") });

            // Act
            var packages = await _sut.FetchUpdgradablePackages(true, cancellationToken);

            // Assert
            Assert.That(packages, Has.Count.EqualTo(expectedNumberOfPackages));
            _processManagerMock.Verify(m => m.ExecuteAsync(Constants.WingetProcessName, "upgrade --include-unknown", cancellationToken), Times.Once);
        }

        [Test]
        [TestCase("0", true)]
        [TestCase("1", false)]
        public async Task ShouldReturnExpectedResultWhenUpgradingAPackage(string exitCode, bool expectedResult)
        {
            // Arrange
            await _sut.UpgradeAsync("node", OnDataReceived, OnErrorRecevied, cancellationToken);

            // Assert
            _processManagerMock.Verify(m => m.StreamAsync(It.IsAny<string>(), "upgrade node", OnDataReceived, OnErrorRecevied, cancellationToken), Times.Once);
        }

        [Test]
        public async Task ShouldReturnExpectedPackageDetails()
        {
            // Arrange
            var packageId = "Discord.Discord";
            var outputLines = new[]
            {
                "  Installer SHA256: 0ad34ac29ecc6dd01f40b46ab0cb1a971b5242aa28ee1a9b72fdb9847c6d6ee1",
                "  Installer Url: https://dl.discordapp.net/distro/app/stable/win/x86/1.0.9007/DiscordSetup.exe",
                "  Installer Locale: en-US",
                "  Installer Type: exe",
                "Installer:",
                "  voip",
                "  voice-chat",
                "  voice",
                "  gaming",
                "  chat",
                "Tags:",
                "Copyright Url: https://discord.com/licenses",
                "Copyright: (c) 2021 Discord Inc.",
                "Privacy Url: https://discord.com/privacy",
                "License Url: https://discord.com/licenses",
                "License: Proprietary",
                "Homepage: https://discord.com",
                "Description: Whether youΓÇÖre part of a school club, gaming group, worldwide art community, or just a handful of friends that want to spend time together, Discord makes it easy to talk every day and hang out more often.",
                "Moniker: discord",
                "Author: Discord Inc.",
                "Publisher Support Url: https://support.discord.com",
                "Publisher Url: https://discord.com",
                "Publisher: Discord Inc.",
                "Version: 1.0.9007",
                "Found Discord [Discord.Discord]",
                "   \b-\b\\\b|\b/\b-\b\\\b|\b/\b-\b\\\b ",
            };
            var processOutput = new ProcessOutput { Output = outputLines };
            var expectedPackageDetails = new PackageDetails
            {
                Version = "1.0.9007",
                Name = "Discord [Discord.Discord]",
                Publisher = "Discord Inc.",
                PublisherUrl = "https://discord.com",
                PublisherSupport = "https://support.discord.com",
                Author = "Discord Inc.",
                Moniker = "discord",
                Description = "Whether youΓÇÖre part of a school club, gaming group, worldwide art community, or just a handful of friends that want to spend time together, Discord makes it easy to talk every day and hang out more often.",
                Homepage = "https://discord.com",
                License = "Proprietary",
                LicenseUrl = "https://discord.com/licenses",
                PrivacyUrl = "https://discord.com/privacy",
                Copyright = "(c) 2021 Discord Inc.",
                CopyrightUrl = "https://discord.com/licenses",
                Tags = new[]
                {
                    "chat",
                    "gaming",
                    "voice",
                    "voice-chat",
                    "voip",
                },
                Installer = new PackageInstaller
                {
                    Type = "exe",
                    Locale = "en-US",
                    Url = "https://dl.discordapp.net/distro/app/stable/win/x86/1.0.9007/DiscordSetup.exe",
                    Hash = "0ad34ac29ecc6dd01f40b46ab0cb1a971b5242aa28ee1a9b72fdb9847c6d6ee1"
                }
            };
            var propertiesToTest = new[]
            {
                nameof(PackageDetails.Version),
                nameof(PackageDetails.Name),
                nameof(PackageDetails.Publisher),
                nameof(PackageDetails.PublisherUrl),
                nameof(PackageDetails.PublisherSupport),
                nameof(PackageDetails.Author),
                nameof(PackageDetails.Moniker),
                nameof(PackageDetails.Description),
                nameof(PackageDetails.Homepage),
                nameof(PackageDetails.License),
                nameof(PackageDetails.LicenseUrl),
                nameof(PackageDetails.PrivacyUrl),
                nameof(PackageDetails.Copyright),
                nameof(PackageDetails.CopyrightUrl),
            };
            _processManagerMock.Setup(m => m.ExecuteAsync(Constants.WingetProcessName, $"show {packageId}", cancellationToken)).ReturnsAsync(new ProcessOutput { Output = outputLines });

            // Act
            var packageDetails = await _sut.FetchPackageDetailsAsync(packageId, cancellationToken);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(PropertyMatcher.Match(expectedPackageDetails, packageDetails, propertiesToTest), Is.True);
                Assert.That(packageDetails.Tags, Is.EqualTo(expectedPackageDetails.Tags));
                Assert.That(PropertyMatcher.Match(expectedPackageDetails.Installer, packageDetails.Installer), Is.True);
                Assert.That(packageDetails.HasValue, Is.True);
            });
            _processManagerMock.Verify(m => m.ExecuteAsync(Constants.WingetProcessName, $"show {packageId}", cancellationToken), Times.Once);
        }

        private void OnDataReceived(string item)
        {
            data.Add(item);
        }

        private void OnErrorRecevied(string item)
        {
            errorData.Add(item);
        }
    }
}
