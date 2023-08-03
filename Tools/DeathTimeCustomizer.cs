using MelonLoader;
using UnityEngine;

namespace PowerTools.Tools
{
    internal static class DeathTimeCustomizer
    {
        private static float _deathTime;

        private static MelonPreferences_Entry<bool> MelonPrefEnabled { get; set; }
        private static bool DeathTimeCustomizerIsEnabled { get; set; }
        private static MelonPreferences_Entry<float> MelonPrefDeathTime { get; set; }


        public static void MelonPreferencesCreator()
        {
            MelonPrefEnabled = Main.MelonPrefCategory.CreateEntry("DeathTimeCustomizerIsEnabled", false);
            MelonPrefDeathTime = Main.MelonPrefCategory.CreateEntry("Damage Threshold", 3f);
            if (MelonPrefEnabled != null )
            {
                DeathTimeCustomizerIsEnabled = MelonPrefEnabled.Value;
            }

            if (MelonPrefDeathTime != null)
            {
                _deathTime = MelonPrefDeathTime.Value;
            }
        }

        public static void BoneMenuCreator()
        {
            var deathTimeCustomizer = Main.Category.CreateCategory("Death Time Customizer", "#FC221b");

            deathTimeCustomizer.CreateBoolElement("Mod Toggle", Color.yellow, DeathTimeCustomizerIsEnabled, OnSetEnabled);
            deathTimeCustomizer.CreateFloatElement("Death Time", "#FC221b", _deathTime, 1f, 0f, 100f, (dt) =>
            {
                _deathTime = dt;
                MelonPrefDeathTime.Value = dt;
                Main.MelonPrefCategory.SaveToFile(false);
                DeathTimeSetter();
            });
        }
        public static void DeathTimeSetter()
        {
            if (BoneLib.Player.rigManager != null && DeathTimeCustomizerIsEnabled)
            {
                BoneLib.Player.rigManager.openControllerRig.playerHealth.deathTimeAmount = _deathTime;
            }
        }

        private static void OnSetEnabled(bool value)
        {
            if (!value)
            {
                BoneLib.Player.rigManager.openControllerRig.playerHealth.deathTimeAmount = 3;
            }
            DeathTimeCustomizerIsEnabled = value;
            MelonPrefEnabled.Value = value;
            Main.MelonPrefCategory.SaveToFile(false);
        }
    }
}
