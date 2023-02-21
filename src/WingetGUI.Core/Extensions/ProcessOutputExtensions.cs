using System.Text.RegularExpressions;
using WingetGUI.Core.Exceptions;
using WingetGUI.Core.Models;

namespace WingetGUI.Core.Extensions
{
    internal static class ProcessOutputExtensions
    {
        internal static IReadOnlyList<PackageSource> ToPackageSources(this ProcessOutput processOutput, int nameRowIndex = 0)
        {
            var packageSources = new List<PackageSource>();
            if (processOutput is not null)
            {
                var output = processOutput.Output.Reverse();
                var columnNameRow = output.ElementAt(nameRowIndex);
                var columnNames = Regex.Replace(columnNameRow, @"\s+", " ").Split(" ");
                var indexes = columnNames.Select(c => columnNameRow.IndexOf(c)).ToArray();
                packageSources = output.Skip(nameRowIndex + 2).Select(source =>
                new PackageSource
                {
                    Name = ExtractValue(source, indexes, 0),
                    Url = ExtractValue(source, indexes, 1),
                }).ToList();
            }
            return packageSources;
        }

        internal static IReadOnlyList<Package> ToPackages(this ProcessOutput processOutput, int nameRowIndex = 0)
        {
            var packages = new List<Package>();
            if (processOutput is not null)
            {
                var output = processOutput.Output.Reverse();
                var columnNameRow = output.ElementAt(nameRowIndex);
                var columnNames = Regex.Replace(columnNameRow, @"\s+", " ").Split(" ");
                var indexes = columnNames.Select(c => columnNameRow.IndexOf(c)).ToArray();
                packages = output.Skip(nameRowIndex + 2).Select(source =>
                new Package
                {
                    Name = ExtractValue(source, indexes, 0),
                    Id = ExtractValue(source, indexes, 1),
                    Version = ExtractValue(source, indexes, 2),
                    Match = ExtractValue(source, indexes, 3),
                    Source = ExtractValue(source, indexes, 4),
                }
                ).ToList();
            }
            return packages;
        }

        internal static IReadOnlyList<UpgradeablePackage> ToUpgradeablePackages(this ProcessOutput processOutput, int nameRowIndex = 0)
        {
            var packages = new List<UpgradeablePackage>();
            if (processOutput is not null)
            {
                var output = processOutput.Output.Reverse();
                var columnNameRow = output.ElementAt(nameRowIndex);
                var columnNames = Regex.Replace(columnNameRow, @"\s+", " ").Split(" ");
                var indexes = columnNames.Select(c => columnNameRow.IndexOf(c)).ToArray();
                packages = output.Skip(nameRowIndex + 2).SkipLast(1).Select(source =>
                new UpgradeablePackage
                {
                    Name = ExtractValue(source, indexes, 0),
                    Id = ExtractValue(source, indexes, 1),
                    InstalledVersion = ExtractValue(source, indexes, 2),
                    AvailableVersion = ExtractValue(source, indexes, 3),
                    Source = ExtractValue(source, indexes, 4),
                }
                ).ToList();
            }
            return packages;
        }

        internal static PackageDetails ToPackageDetails(this ProcessOutput processOutput)
        {
            var output = processOutput.Output.Reverse();
            if (output.Count() <= 2) throw new PackageNotFoundException();

            int installerLine = FindIndex(output, "Installer:"),
                tagsLine = FindIndex(output, "Tags:");
            return new PackageDetails
            {
                Version = ExtractValue(output, "Version: "),
                Name = ExtractValue(output, "Found "),
                Description = ExtractValue(output, "Description: "),
                Publisher = ExtractValue(output, "Publisher: "),
                PublisherUrl = ExtractValue(output, "Publisher Url: "),
                PublisherSupport = ExtractValue(output, "Publisher Support Url: "),
                Author = ExtractValue(output, "Author: "),
                Moniker = ExtractValue(output, "Moniker: "),
                Homepage = ExtractValue(output, "Homepage: "),
                License = ExtractValue(output, "License: "),
                LicenseUrl = ExtractValue(output, "License Url: "),
                PrivacyUrl = ExtractValue(output, "Privacy Url: "),
                Copyright = ExtractValue(output, "Copyright: "),
                CopyrightUrl = ExtractValue(output, "Copyright Url: "),
                Tags = ExtractTags(output, tagsLine, installerLine).ToList(),
                Installer = new PackageInstaller
                {
                    Type = ExtractValue(output, "  Installer Type: "),
                    Locale = ExtractValue(output, "  Installer Locale: "),
                    Url = ExtractValue(output, "  Installer Url: "),
                    Hash = ExtractValue(output, "  Installer SHA256: "),
                }
            };
        }

        internal static IReadOnlyList<Feature> ToFeatures(this ProcessOutput processOutput, int nameRowIndex = 0)
        {
            var features = new List<Feature>();
            if (processOutput is not null)
            {
                var output = processOutput.Output.Reverse();
                var columnNameRow = output.ElementAt(nameRowIndex);
                var columnNames = Regex.Replace(columnNameRow, @"\s+", " ").Split(" ");
                var indexes = columnNames.Select(c => columnNameRow.IndexOf(c)).ToArray();
                features = output.Skip(nameRowIndex + 2).Select(source =>
                new Feature
                {
                    Name = ExtractValue(source, indexes, 0),
                    Enabled = !"Disabled".Equals(ExtractValue(source, indexes, 1)),
                    Property = ExtractValue(source, indexes, 2),
                    Link = ExtractValue(source, indexes, 3),
                }).ToList();
            }
            return features;
        }

        private static IEnumerable<string> ExtractTags(IEnumerable<string> outputs, int tagIndex, int installerIndex)
        {
            return outputs.Where((s, i) => i > tagIndex && i < installerIndex).Select(t => t.Trim());
        }

        private static int FindIndex(IEnumerable<string> outputs, string pattern)
        {
            var line = outputs.First(o => o.StartsWith(pattern));
            return Array.IndexOf(outputs.ToArray(), line);
        }

        private static string ExtractValue(IEnumerable<string> outputs, string pattern)
        {
            var lineIndex = FindIndex(outputs, pattern);
            var line = outputs.ElementAt(lineIndex);
            return line.Replace(pattern, "").Trim();
        }

        private static string ExtractValue(string source, int[] indexes, int index)
        {
            if (index is 0) return source[0..indexes[1]].Trim();
            if (index == indexes.Length - 1) return source[indexes[^1]..].Trim();
            return source[indexes[index]..indexes[index + 1]].Trim();
        }
    }
}
