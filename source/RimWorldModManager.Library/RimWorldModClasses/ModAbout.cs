using System.Xml;

namespace RimWorldModManager.Library.RimWorldModClasses;

public class ModAbout
{
    //TODO: Add basic summaries based on https://rimworldwiki.com/wiki/Modding_TutorialsModMetaData.xml
    public string PackageId { get; set; }
    public string ModName { get; set; }
    public List<string> Authors { get; set; }
    public string Description { get; set; }
    public List<string> SupportedVersions { get; set; }
    public string? ModVersion { get; set; }
    public string? Url { get; set; }
    public List<VersionDescription>? VersionDescription { get; set; }
    public List<ModDependency>? Dependencies { get; set; }
    public Dictionary<string, List<ModDependency>> DependencyByVersions { get; set; }
    public List<string>? LoadBefore { get; set; }
    public Dictionary<string, List<string>> LoadBeforeByVersions { get; set; }
    public List<string>? LoadAfter { get; set; }
    public Dictionary<string, List<string>> LoadAfterByVersions { get; set; }
    public List<string>? ForceLoadBefore { get; set; }
    public List<string>? ForceLoadAfter { get; set; }
    public List<string>? IncompatibleWith { get; set; }
    public Dictionary<string, List<string>> IncompatibleWithByVersions { get; set; }

    public ModAbout(XmlDocument xmlDocument)
    {
        var packageId = xmlDocument.SelectSingleNode("ModMetaData/packageId");
        if (packageId != null)
        {
            PackageId = packageId.InnerText;
        }

        var modName = xmlDocument.SelectSingleNode("ModMetaData/name");
        if (modName != null)
        {
            ModName = modName.InnerText;
        }
        
        if(xmlDocument.SelectSingleNode("ModMetaData/author") != null)
        {
            var authorString = xmlDocument.SelectSingleNode("ModMetaData/author").InnerText;
            if(authorString.Contains(","))
            {
                Authors = authorString.Split(",").Select(x => x.Trim()).ToList();
            }
            else
            {
                Authors = new List<string>() {authorString};
            }
        }
        else if(xmlDocument.SelectSingleNode("ModMetaData/authors") != null)
        {
            Authors = new List<string>();
            foreach (XmlNode author in xmlDocument.SelectNodes("ModMetaData/authors/li"))
            {
                Authors.Add(author.InnerText);
            }
        }

        var description = xmlDocument.SelectSingleNode("ModMetaData/description");
        if (description != null)
        {
            Description = description.InnerText;
        }

        var supportedVersions = xmlDocument.SelectNodes("ModMetaData/supportedVersions/li");
        if (supportedVersions != null)
        {
            SupportedVersions = new List<string>();
            foreach (XmlNode supportedVersion in supportedVersions)
            {
                SupportedVersions.Add(supportedVersion.InnerText);
            }
        }

        var modVersion = xmlDocument.SelectSingleNode("ModMetaData/modVersion");
        if (modVersion != null)
        {
            ModVersion = modVersion.InnerText;
        }

        var url = xmlDocument.SelectSingleNode("ModMetaData/url");
        if (url != null)
        {
            Url = url.InnerText;
        }

        // There might be an issue where the version is not in the list of supported versions and it will be skipped
        foreach (var version in SupportedVersions)
        {
            try
            {
                var versionDescription = xmlDocument.SelectSingleNode($"ModMetaData/descriptionsByVersion/v{version}");
                if (versionDescription != null)
                {
                    if (VersionDescription == null)
                    {
                        VersionDescription = new List<VersionDescription>();
                    }

                    VersionDescription.Add(new VersionDescription()
                    {
                        Version = version,
                        Description = versionDescription.InnerText
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error parsing version description");
            }
        }

        var dependencies = xmlDocument.SelectNodes("ModMetaData/modDependencies/li");
        if (dependencies != null)
        {
            Dependencies = new List<ModDependency>();
            foreach (XmlNode dependency in dependencies)
            {
                var dependencyPackageId = dependency.SelectSingleNode("packageId");
                var displayName = dependency.SelectSingleNode("displayName");
                var steamWorkshopUrl = dependency.SelectSingleNode("steamWorkshopUrl");
                if (dependencyPackageId != null && displayName != null)
                {
                    Dependencies.Add(new ModDependency()
                    {
                        PackageId = dependencyPackageId.InnerText,
                        DisplayName = displayName.InnerText,
                        SteamWorkshopUrl = steamWorkshopUrl?.InnerText
                    });
                }
            }
        }
        
        foreach (var version in SupportedVersions)
        {
            try
            {
                var dependenciesByVersion = xmlDocument.SelectNodes($"ModMetaData/modDependenciesByVersion/v{version}/li");
                if (dependenciesByVersion.Count != 0)
                {
                    if (DependencyByVersions == null)
                    {
                        DependencyByVersions = new Dictionary<string, List<ModDependency>>();
                    }

                    DependencyByVersions.Add(version, new List<ModDependency>());
                    foreach (XmlNode dependencyByVersion in dependenciesByVersion)
                    {
                        var dependencyPackageId = dependencyByVersion.SelectSingleNode("packageId");
                        var displayName = dependencyByVersion.SelectSingleNode("displayName");
                        var steamWorkshopUrl = dependencyByVersion.SelectSingleNode("steamWorkshopUrl");
                        if (dependencyPackageId != null && displayName != null)
                        {
                            DependencyByVersions[version].Add(new ModDependency()
                            {
                                PackageId = dependencyPackageId.InnerText,
                                DisplayName = displayName.InnerText,
                                SteamWorkshopUrl = steamWorkshopUrl?.InnerText
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error parsing version dependency");
            }
        }
        
        var loadBefore = xmlDocument.SelectNodes("ModMetaData/loadBefore/li");
        if (loadBefore != null)
        {
            LoadBefore = new List<string>();
            foreach (XmlNode loadBeforeNode in loadBefore)
            {
                LoadBefore.Add(loadBeforeNode.InnerText);
            }
        }
        
        foreach (var version in SupportedVersions)
        {
            try
            {
                var loadBeforeByVersion = xmlDocument.SelectNodes($"ModMetaData/loadBeforeByVersion/v{version}/li");
                if (loadBeforeByVersion.Count != 0)
                {
                    if (LoadBeforeByVersions == null)
                    {
                        LoadBeforeByVersions = new Dictionary<string, List<string>>();
                    }

                    LoadBeforeByVersions.Add(version, new List<string>());
                    foreach (XmlNode loadBeforeNode in loadBeforeByVersion)
                    {
                        LoadBeforeByVersions[version].Add(loadBeforeNode.InnerText);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error parsing load before by version");
            }
        }
        
        var loadAfter = xmlDocument.SelectNodes("ModMetaData/loadAfter/li");
        if (loadAfter != null)
        {
            LoadAfter = new List<string>();
            foreach (XmlNode loadAfterNode in loadAfter)
            {
                LoadAfter.Add(loadAfterNode.InnerText);
            }
        }
        
        foreach (var version in SupportedVersions)
        {
            try
            {
                var loadAfterByVersion = xmlDocument.SelectNodes($"ModMetaData/loadAfterByVersion/v{version}/li");
                if (loadAfterByVersion.Count != 0)
                {
                    if (LoadAfterByVersions == null)
                    {
                        LoadAfterByVersions = new Dictionary<string, List<string>>();
                    }

                    LoadAfterByVersions.Add(version, new List<string>());
                    foreach (XmlNode loadAfterNode in loadAfterByVersion)
                    {
                        LoadAfterByVersions[version].Add(loadAfterNode.InnerText);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error parsing load after by version");
            }
        }
        
        var forceLoadBefore = xmlDocument.SelectNodes("ModMetaData/forceLoadBefore/li");
        if (forceLoadBefore != null)
        {
            ForceLoadBefore = new List<string>();
            foreach (XmlNode forceLoadBeforeNode in forceLoadBefore)
            {
                ForceLoadBefore.Add(forceLoadBeforeNode.InnerText);
            }
        }
        
        var forceLoadAfter = xmlDocument.SelectNodes("ModMetaData/forceLoadAfter/li");
        if (forceLoadAfter != null)
        {
            ForceLoadAfter = new List<string>();
            foreach (XmlNode forceLoadAfterNode in forceLoadAfter)
            {
                ForceLoadAfter.Add(forceLoadAfterNode.InnerText);
            }
        }
        
        var incompatibleWith = xmlDocument.SelectNodes("ModMetaData/incompatibleWith/li");
        if (incompatibleWith != null)
        {
            IncompatibleWith = new List<string>();
            foreach (XmlNode incompatibleWithNode in incompatibleWith)
            {
                IncompatibleWith.Add(incompatibleWithNode.InnerText);
            }
        }
        
        foreach (var version in SupportedVersions)
        {
            try
            {
                var incompatibleWithByVersion = xmlDocument.SelectNodes($"ModMetaData/incompatibleWithByVersion/v{version}/li");
                if (incompatibleWithByVersion.Count != 0)
                {
                    if (IncompatibleWithByVersions == null)
                    {
                        IncompatibleWithByVersions = new Dictionary<string, List<string>>();
                    }

                    IncompatibleWithByVersions.Add(version, new List<string>());
                    foreach (XmlNode incompatibleWithNode in incompatibleWithByVersion)
                    {
                        IncompatibleWithByVersions[version].Add(incompatibleWithNode.InnerText);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error parsing incompatible with by version");
            }
        }

        Console.WriteLine();
    }
}