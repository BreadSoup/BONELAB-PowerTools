using System.Reflection;
using PowerTools;
using MelonLoader;

[assembly: AssemblyTitle(PowerTools.Main.Description)]
[assembly: AssemblyDescription(PowerTools.Main.Description)]
[assembly: AssemblyCompany(PowerTools.Main.Company)]
[assembly: AssemblyProduct(PowerTools.Main.Name)]
[assembly: AssemblyCopyright("Developed by " + PowerTools.Main.Author)]
[assembly: AssemblyTrademark(PowerTools.Main.Company)]
[assembly: AssemblyVersion(PowerTools.Main.Version)]
[assembly: AssemblyFileVersion(PowerTools.Main.Version)]
[assembly: MelonInfo(typeof(PowerTools.Main), PowerTools.Main.Name, PowerTools.Main.Version, PowerTools.Main.Author, PowerTools.Main.DownloadLink)]
[assembly: MelonColor(System.ConsoleColor.Cyan)]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("Stress Level Zero", "BONELAB")]