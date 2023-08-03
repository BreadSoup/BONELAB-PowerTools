using MelonLoader;
using UnityEngine;

namespace PowerTools.Tools
{
    public static class ReloadOnDeathCustomizer
    {
        private static MelonPreferences_Entry<bool> MelonPrefEnabled { get; set; }
        private static bool ReloadOnDeathCustomizerIsEnabled { get; set; }
        private static MelonPreferences_Entry<bool> MelonPrefReloadLevelOnDeath { get; set; }
        public static bool ReloadLevel;
        public static bool IsDefaultSet { get; set; }
        private static bool _defaultReloadOnDeathSettingValue;

        public static void MelonPreferencesCreator()
        {
            MelonPrefEnabled = Main.MelonPrefCategory.CreateEntry("ReloadOnDeathCustomizerIsEnabled", false);
            MelonPrefReloadLevelOnDeath = Main.MelonPrefCategory.CreateEntry("Reload Level On Death", false);
            if (MelonPrefEnabled != null)
            {
                ReloadOnDeathCustomizerIsEnabled = MelonPrefEnabled.Value;
            }

            if (MelonPrefReloadLevelOnDeath != null)
            {
                ReloadLevel = MelonPrefReloadLevelOnDeath.Value;
            }
        }



        public static void BoneMenuCreator()
        {
            var reloadOnDeathCustomizer = Main.Category.CreateCategory("Reload On Death Customizer", "#ff6f00");

            reloadOnDeathCustomizer.CreateBoolElement("Mod Toggle", Color.yellow, ReloadOnDeathCustomizerIsEnabled,
                OnSetEnabled);
            reloadOnDeathCustomizer.CreateBoolElement("Reload Level On Death", "#ff3700", ReloadLevel, ReloadOnDeathSetter);
        }

        public static void ReloadOnDeathSetter(bool value)
        {
            if (!IsDefaultSet)
            {
                _defaultReloadOnDeathSettingValue = BoneLib.Player.rigManager.openControllerRig.playerHealth.reloadLevelOnDeath;
                IsDefaultSet = true;
            }
            if (BoneLib.Player.rigManager != null && ReloadOnDeathCustomizerIsEnabled)
            {
                BoneLib.Player.rigManager.openControllerRig.playerHealth.reloadLevelOnDeath = ReloadLevel;
            }
            MelonPrefReloadLevelOnDeath.Value = ReloadLevel;
            Main.MelonPrefCategory.SaveToFile(false);
        }

        private static void OnSetEnabled(bool value)
        {
            if (!value)
            {
                BoneLib.Player.rigManager.openControllerRig.playerHealth.reloadLevelOnDeath = _defaultReloadOnDeathSettingValue;
            }
            else
            {
                ReloadOnDeathSetter(ReloadLevel);
            }

            ReloadOnDeathCustomizerIsEnabled = value;
            MelonPrefEnabled.Value = value;
            Main.MelonPrefCategory.SaveToFile(false);
        }
    }
}