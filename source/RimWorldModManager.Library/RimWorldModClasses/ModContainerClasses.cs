namespace RimWorldModManager.Library.RimWorldModClasses;

public class VersionDescription
{
    public string Version { get; set; }
    public string Description { get; set; }
}

public class ModDependency
{
    public string PackageId { get; set; }
    public string DisplayName { get; set; }
    public string? SteamWorkshopUrl { get; set; }
}
