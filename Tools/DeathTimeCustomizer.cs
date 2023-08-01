using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BoneLib;
using MelonLoader;
using UnityEngine;

namespace PowerTools.Tools
{
    internal class DeathTimeCustomizer
    {
        static float DeathTime;

        public static MelonPreferences_Entry<bool> MelonPrefEnabled { get; private set; }
        public static bool IsEnabled { get; private set; }
        public static MelonPreferences_Entry<float> MelonPrefDeathTime { get; private set; }

        public static void MelonPreferencesCreator()
        {
            MelonPrefEnabled = Main.MelonPrefCategory.CreateEntry<bool>("IsEnabled", false, null, null, false, false, null, null);
            IsEnabled = MelonPrefEnabled.Value;
            MelonPrefDeathTime = Main.MelonPrefCategory.CreateEntry<float>("Damage Threshold", 3f, null, null, false, false, null, null);

            if (MelonPrefDeathTime != null)
            {
                DeathTime = MelonPrefDeathTime.Value;
            }
        }

        public static void BonemenuCreator()
        {
            var DeathTimeCustomizer = Main.category.CreateCategory("Death Time Customizer", "#FC221b");

            DeathTimeCustomizer.CreateBoolElement("Mod Toggle", Color.yellow, IsEnabled, new Action<bool>(OnSetEnabled));
            var DamageThreshold = DeathTimeCustomizer.CreateFloatElement("Death Time", "#FC221b", DeathTime, 1f, 0f, 100f, (dt) =>
            {
                DeathTime = dt;
                MelonPrefDeathTime.Value = dt;
                Main.MelonPrefCategory.SaveToFile(false);
                DeathTimeSetter();
            });
        }
        public static void DeathTimeSetter()
        {
            if (BoneLib.Player.rigManager != null && IsEnabled)
            {
                BoneLib.Player.rigManager.openControllerRig.playerHealth.deathTimeAmount = DeathTime;
            }
        }

        public static void OnSetEnabled(bool value)
        {
            if (!value)
            {
                BoneLib.Player.rigManager.openControllerRig.playerHealth.deathTimeAmount = 3;
            }
            IsEnabled = value;
            MelonPrefEnabled.Value = value;
            Main.MelonPrefCategory.SaveToFile(false);
        }
    }
}
