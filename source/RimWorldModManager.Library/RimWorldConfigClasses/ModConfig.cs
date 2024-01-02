using System.Xml.Serialization;

namespace RimWorldModManager.Library.RimWorldConfigClasses;

[XmlRoot(ElementName="activeMods")]
public class ActiveMods { 

    [XmlElement(ElementName="li")] 
    public List<string> CurrentlyActiveMods { get; set; } 
}

[XmlRoot(ElementName="knownExpansions")]
public class KnownExpansions { 

    [XmlElement(ElementName="li")] 
    public List<string> Expansions { get; set; } 
}

[XmlRoot(ElementName="ModsConfigData")]
public class ModsConfigData { 

    [XmlElement(ElementName="version")] 
    public string Version { get; set; } 

    [XmlElement(ElementName="activeMods")] 
    public ActiveMods ActiveMods { get; set; } 

    [XmlElement(ElementName="knownExpansions")] 
    public KnownExpansions KnownExpansions { get; set; } 
}