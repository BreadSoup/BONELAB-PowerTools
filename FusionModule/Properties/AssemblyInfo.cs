using System.Reflection;
using PowerToolsFusionModule;
using MelonLoader;

[assembly: AssemblyTitle(PowerToolsFusionModule.Main.Description)]
[assembly: AssemblyDescription(PowerToolsFusionModule.Main.Description)]
[assembly: AssemblyCompany(PowerToolsFusionModule.Main.Company)]
[assembly: AssemblyProduct(PowerToolsFusionModule.Main.Name)]
[assembly: AssemblyCopyright("Developed by " + PowerToolsFusionModule.Main.Author)]
[assembly: AssemblyTrademark(PowerToolsFusionModule.Main.Company)]
[assembly: AssemblyVersion(PowerToolsFusionModule.Main.Version)]
[assembly: AssemblyFileVersion(PowerToolsFusionModule.Main.Version)]
[assembly: MelonInfo(typeof(PowerToolsFusionModule.Main), PowerToolsFusionModule.Main.Name, PowerToolsFusionModule.Main.Version, PowerToolsFusionModule.Main.Author, PowerToolsFusionModule.Main.DownloadLink)]
[assembly: MelonColor(System.ConsoleColor.Magenta)]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("Stress Level Zero", "BONELAB")]