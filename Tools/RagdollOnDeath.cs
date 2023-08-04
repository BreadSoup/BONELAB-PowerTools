

using MelonLoader;
using UnityEngine;

namespace PowerTools.Tools
{
    public static class RagdollOnDeath
    {
        private static MelonPreferences_Entry<bool> MelonPrefEnabled { get; set; }
        public static bool RagdollOnDeathIsEnabled { get; private set; }

        public static void MelonPreferencesCreator()
        {
            MelonPrefEnabled = Main.MelonPrefCategory.CreateEntry("RagdollOnDeathIsEnabled", false);
            if (MelonPrefEnabled != null)
            {
                RagdollOnDeathIsEnabled = MelonPrefEnabled.Value;
            }
        }

        public static void BoneMenuCreator()
        {
            var ragdollOnDeathCustomizer = Main.Category.CreateCategory("Ragdoll On Death", "#00fc82");

            ragdollOnDeathCustomizer.CreateBoolElement("Mod Toggle", Color.yellow, RagdollOnDeathIsEnabled, OnSetEnabled);
        }


        public static void OnSetEnabled(bool value)
        {
            BoneLib.Player.rigManager.health._testRagdollOnDeath = value;
            RagdollOnDeathIsEnabled = value;
            MelonPrefEnabled.Value = value;
            Main.MelonPrefCategory.SaveToFile(false);
        }
    }
}