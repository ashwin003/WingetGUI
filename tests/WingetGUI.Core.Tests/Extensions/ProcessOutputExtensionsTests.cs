using WingetGUI.Core.Exceptions;
using WingetGUI.Core.Extensions;
using WingetGUI.Core.Tests.Utils;

namespace WingetGUI.Core.Tests.Extensions
{
    public class ProcessOutputExtensionsTests
    {
        [Test]
        public void ShouldReturnExpectedListOfPackageSources()
        {
            // Arrange
            var expectedResult = new List<PackageSource>
            {
                new PackageSource
                {
                    Name = "msstore",
                    Url = "https://storeedgefd.dsx.mp.microsoft.com/v9.0"
                },
                new PackageSource
                {
                    Name = "winget",
                    Url = "https://cdn.winget.microsoft.com/cache"
                }
            };
            var processOutput = new ProcessOutput
            {
                Output = new List<string>
                {
                    "winget  https://cdn.winget.microsoft.com/cache",
                    "msstore https://storeedgefd.dsx.mp.microsoft.com/v9.0",
                    "-----------------------------------------------------",
                    "Name    Argument",
                }
            };

            // Act
            var sources = processOutput.ToPackageSources();

            // Assert
            Assert.That(CollectionMatcher.Match(expectedResult, sources), Is.True);
        }

        [Test]
        public void ShouldReturnExpectedListOfPackages()
        {
            // Arrange
            var expectedResult = new List<Package>
            {
                new Package
                {
                    Name = "Chia Blockchain",
                    Id = "ChiaNetwork.GUIforChiaBlockchain",
                    Version = "1.6.0",
                    Match = "Tag: full-node",
                    Source = "winget"
                },
                new Package
                {
                    Name = "Cactus Blockchain",
                    Id = "CactusNetwork.CactusBlockchainGUI",
                    Version = "1.3.4",
                    Match = "Tag: full-node",
                    Source = "winget"
                },
            };
            var processOutput = new ProcessOutput
            {
                Output = new List<string>
                {
                    "Cactus Blockchain                             CactusNetwork.CactusBlockchainGUI 1.3.4            Tag: full-node winget",
                    "Chia Blockchain                               ChiaNetwork.GUIforChiaBlockchain  1.6.0            Tag: full-node winget",
                    "-----------------------------------------------------------------------------------------------------------------------",
                    "Name                                          Id                                Version          Match          Source",
                    "   \b-\b\\\b|\b/\b-\b\\\b|\b "
                }
            };

            // Act
            var packages = processOutput.ToPackages(1);

            // Assert
            Assert.That(CollectionMatcher.Match(expectedResult, packages), Is.True);
        }

        [Test]
        public void ShouldReturnExpectedListOfUpgradeablePackages()
        {
            // Arrange
            var expectedResult = new List<UpgradeablePackage>
            {
                new UpgradeablePackage
                {
                    Name = "Android Studio",
                    Id = "Google.AndroidStudio.Canary",
                    InstalledVersion = "2022.2.1.1",
                    AvailableVersion = "2022.2.1.6",
                    Source = "winget"
                },
                new UpgradeablePackage
                {
                    Name = "Discord",
                    Id = "Discord.Discord",
                    InstalledVersion = "1.0.9005",
                    AvailableVersion = "1.0.9007",
                    Source = "winget"
                },
            };
            var processOutput = new ProcessOutput
            {
                Output = new List<string>
                {
                    "2 upgrades available.",
                    "Discord                                             Discord.Discord                1.0.9005         1.0.9007     winget",
                    "Android Studio                                      Google.AndroidStudio.Canary    2022.2.1.1       2022.2.1.6   winget",
                    "-----------------------------------------------------------------------------------------------------------------------",
                    "Name                                                Id                             Version          Available    Source",
                    "   \b-\b ",
                    "   \b-\b\\\b|\b/\b "
                }
            };

            // Act
            var packages = processOutput.ToUpgradeablePackages(2);

            // Assert
            Assert.That(CollectionMatcher.Match(expectedResult, packages), Is.True);
        }

        [Test]
        public void ShouldReturnPackageDetailsFromProcessOutput()
        {
            // Arrange
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

            // Act
            var packageDetails = processOutput.ToPackageDetails();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(PropertyMatcher.Match(expectedPackageDetails, packageDetails, propertiesToTest), Is.True);
                Assert.That(packageDetails.Tags, Is.EqualTo(expectedPackageDetails.Tags));
                Assert.That(PropertyMatcher.Match(expectedPackageDetails.Installer, packageDetails.Installer), Is.True);
            });
        }

        [Test]
        public void ShouldThrowExceptionWhenPackageIsNotFound()
        {
            // Arrange
            var outputLines = new[]
            {
                "No package found matching input criteria.",
                "   \b-\b\\\b|\b "
            };
            var processOutput = new ProcessOutput { Output = outputLines };

            // Act & Assert
            Assert.Throws<PackageNotFoundException>(() => processOutput.ToPackageDetails());
        }

        [Test]
        public void ShouldReturnExpectedFeatures()
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
            };
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

            // Act
            var features = processOutput.ToFeatures(2);

            // Assert
            Assert.That(CollectionMatcher.Match(expectedFeatures, features), Is.True);
        }
    }
}
