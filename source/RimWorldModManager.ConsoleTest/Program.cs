using System.Xml;
using RimWorldModManager.Library.RimWorldModClasses;

public class Program
{
    public static void Main(string[] args)
    {
        
        string steamWorkshopDirectory = "";
        
        var modData = new List<ModAbout>();

        foreach (var directory in Directory.GetDirectories(steamWorkshopDirectory))
        {
            try
            {
                var xml = File.ReadAllText($"{directory}\\About\\About.xml");
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                ModAbout modAbout = new ModAbout(xmlDocument);
                modData.Add(modAbout);
            }
            catch (Exception e)
            {
                Console.WriteLine("Missing About.xml");
            }
        }

    }
}