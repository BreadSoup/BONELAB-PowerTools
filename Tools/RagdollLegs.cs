using BoneLib;
using UnityEngine;
using UnityEngine.UI;

namespace PowerTools.Tools
{
    internal static class RagdollLegs
    {
        //need to make a melody patch or whatever so it doesn't rest when unragdolling from something like ragdoll mod
        public static void BoneMenuCreator()
        {
            var ragdollLegs = Main.Category.CreateCategory("Ragdoll Legs", "#51fc5a");
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