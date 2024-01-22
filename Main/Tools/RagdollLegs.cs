using BoneLib;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace PowerTools.Tools
{
    internal static class RagdollLegs
    {
        //need to make a melody patch or whatever so it doesn't rest when unragdolling from something like ragdoll mod
        //what did I mean by this top comment??????????????
        
        private static bool VaultingToggleIsEnabled { get; set; }
        private static MelonPreferences_Entry<bool> MelonPrefRagdollLegs { get; set; }
        
        public static void MelonPreferencesCreator()
        {
            MelonPrefRagdollLegs = Main.MelonPrefCategory.CreateEntry("Vaulting Toggle", true);

            if (MelonPrefRagdollLegs != null)
            {
                VaultingToggleIsEnabled = MelonPrefRagdollLegs.Value;
                OnSetEnabled(VaultingToggleIsEnabled);
            }
        }
        public static void BoneMenuCreator()
        {
            var ragdollLegs = Main.Category.CreateCategory("Ragdoll Legs", "#ffa040");
            ragdollLegs.CreateBoolElement("Mod Toggle", Color.yellow, _isEnabled, OnSetEnabled);

        }
        private static void OnSetEnabled(bool value)
        {
            _isEnabled = value;
            if (value)
            {
                Player.physicsRig.PhysicalLegs();
            }
            else
            {
                Player.physicsRig.UnRagdollRig();
            }
        }
        private static bool _isEnabled;
    }
}