using HarmonyLib;
using MelonLoader;
using SLZ.Rig;

namespace PowerTools.Tools
{
    [HarmonyPatch(typeof(PhysicsRig), "CheckDangle")]
    public class VaultingToggle
    {
        private static bool VaultingToggleIsEnabled { get; set; }
        private static MelonPreferences_Entry<bool> MelonPrefVaultingToggle { get; set; }

        public static void MelonPreferencesCreator()
        {
            MelonPrefVaultingToggle = Main.MelonPrefCategory.CreateEntry("Vaulting Toggle", true);

            if (MelonPrefVaultingToggle != null)
            {
                VaultingToggleIsEnabled = MelonPrefVaultingToggle.Value;
            }
        }

        public static void BoneMenuCreator()
        {
            var vaultingToggle = Main.Category.CreateCategory("Vaulting Toggle", "#7526fc"); 

            vaultingToggle.CreateBoolElement("Vaulting", "#6d45ff", VaultingToggleIsEnabled, OnSetEnabled);
        }
        
        public static bool Prefix(PhysicsRig __instance, ref bool __result) // DO NOT CHANGE __instance OR __result TO ANYTHING ELSE
        {
            if(!VaultingToggleIsEnabled)
            {
                __result = false;
                return false;
            }
            else
            {
                return true;
            }
        }

        private static void OnSetEnabled(bool value)
        {
            VaultingToggleIsEnabled = value;
            Main.MelonPrefCategory.SaveToFile(false);
        }
    }
}