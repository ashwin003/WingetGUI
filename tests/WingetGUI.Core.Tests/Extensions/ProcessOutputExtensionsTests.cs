using System;
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
                    "Name    Argument",
                    "-----------------------------------------------------",
                    "msstore https://storeedgefd.dsx.mp.microsoft.com/v9.0",
                    "winget  https://cdn.winget.microsoft.com/cache",
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
                    Name = "Blender",
                    Id = "9PP3C07GTVRH",
                    Version = "Unknown",
                    Source = "msstore"
                },
                new Package
                {
                    Name = "Blender",
                    Id = "BlenderFoundation.Blender",
                    Version = "3.5.0",
                    Source = "winget"
                },
            };
            var processOutput = new ProcessOutput
            {
                Output = new List<string>
                {
                    "Name                                             Id                        Version Source",
                    "-------------------------------------------------------------------------------------------",
                    "Blender                                          9PP3C07GTVRH              Unknown msstore",
                    "Blender                                          BlenderFoundation.Blender 3.5.0   winget",
                    "",
                }
            };

            // Act
            var packages = processOutput.ToPackages(0);

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
                    "Name                                                   Id                           Version        Available     Source",
                    "-----------------------------------------------------------------------------------------------------------------------",
                    "Android Studio                                         Google.AndroidStudio.Canary    2022.2.1.1       2022.2.1.6   winget",
                    "Discord                                                Discord.Discord                1.0.9005         1.0.9007     winget",
                    "2 upgrades available.",
                }
            };

            // Act
            var packages = processOutput.ToUpgradeablePackages(0);

            // Assert
            Assert.That(CollectionMatcher.Match(expectedResult, packages), Is.True);
        }

        [Test]
        public void ShouldReturnPackageDetailsFromProcessOutput()
        {
            // Arrange
            var outputLines = new[]
            {
                "Found Discord [Discord.Discord]",
"Version: 1.0.9011",
"Publisher: Discord Inc.",
"Publisher Url: https://discord.com",
"Publisher Support Url: https://support.discord.com",
"Author: Discord Inc.",
"Moniker: discord",
"Description: Whether youΓÇÖre part of a school club, gaming group, worldwide art community, or just a handful of friends that want to spend time together, Discord makes it easy to talk every day and hang out more often.",
"Homepage: https://discord.com",
"License: Proprietary",
"License Url: https://discord.com/licenses",
"Privacy Url: https://discord.com/privacy",
"Copyright: (c) 2021 Discord Inc.",
"Copyright Url: https://discord.com/licenses",
"Tags:",
"  chat",
"  gaming",
"  voice",
"  voice-chat",
"  voip",
"Installer:",
            "  Installer Type: exe",
"  Installer Locale: en-US",
"  Installer Url: https://dl.discordapp.net/distro/app/stable/win/x86/1.0.9011/DiscordSetup.exe",
"  Installer SHA256: d7d109e6d48cbc08320b4af0ed2080826699867c80803fec9e49ce0cf47d2d0f",
""
            };
            var processOutput = new ProcessOutput { Output = outputLines };
            var expectedPackageDetails = new PackageDetails
            {
                Version = "1.0.9011",
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
                    Url = "https://dl.discordapp.net/distro/app/stable/win/x86/1.0.9011/DiscordSetup.exe",
                    Hash = "d7d109e6d48cbc08320b4af0ed2080826699867c80803fec9e49ce0cf47d2d0f"
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
                "The following experimental features are in progress.",
                "They can be configured through the settings file 'winget settings'.",
                "",
                "Feature                       Status   Property                  Link",
                "-----------------------------------------------------------------------------------------------",
                "Show Dependencies Information Disabled dependencies              https://aka.ms/winget-settings",
                "Direct MSI Installation       Disabled directMSI                 https://aka.ms/winget-settings",
                "Package Pinning               Disabled pinning                   https://aka.ms/winget-settings",
                "Uninstall Previous Argument   Disabled uninstallPreviousArgument https://aka.ms/winget-settings",
                ""
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
                    Name= "Package Pinning",
                    Enabled = false,
                    Property = "pinning",
                    Link = "https://aka.ms/winget-settings"
                },
                new Feature
                {
                    Name= "Uninstall Previous Argument",
                    Enabled = false,
                    Property = "uninstallPreviousArgument",
                    Link = "https://aka.ms/winget-settings"
                },
            };

            // Act
            var features = processOutput.ToFeatures(3);

            // Assert
            Assert.That(CollectionMatcher.Match(expectedFeatures, features), Is.True);
        }
    }
}
