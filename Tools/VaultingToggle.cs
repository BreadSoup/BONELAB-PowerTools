using HarmonyLib;
using MelonLoader;
using Mono.CSharp;
using SLZ.Rig;
using UnityEngine;

namespace PowerTools.Tools
{
    [HarmonyPatch(typeof(PhysicsRig), "CheckDangle")]
    public class VaultingToggle
    {
        private static MelonPreferences_Entry<bool> MelonPrefEnabled { get; set; }
        private static bool VaultingToggleIsEnabled { get; set; }
        private static MelonPreferences_Entry<bool> MelonPrefVaultingToggle { get; set; }
        public static bool VaultingToggleSetting;
        public static bool IsDefaultSet { get; set; }

        public static void MelonPreferencesCreator()
        {
            MelonPrefEnabled = Main.MelonPrefCategory.CreateEntry("VaultingToggleIsEnabled", false);
            MelonPrefVaultingToggle = Main.MelonPrefCategory.CreateEntry("Vaulting Toggle", false);
            if (MelonPrefEnabled != null)
            {
                VaultingToggleIsEnabled = MelonPrefEnabled.Value;
            }

            if (MelonPrefVaultingToggle != null)
            {
                VaultingToggleSetting = MelonPrefVaultingToggle.Value;
            }
        }

        public static void BoneMenuCreator()
        {
            var vaultingToggle = Main.Category.CreateCategory("Vaulting Toggle", "#7526fc"); 

            vaultingToggle.CreateBoolElement("Mod Toggle", Color.yellow, VaultingToggleIsEnabled, OnSetEnabled);
        }
        
        public static bool Prefix(PhysicsRig __instance, ref bool __result)
        {
            if(VaultingToggleIsEnabled)
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
            MelonPrefEnabled.Value = value;
            Main.MelonPrefCategory.SaveToFile(false);
        }
    }
}