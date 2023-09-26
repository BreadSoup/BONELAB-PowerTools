using BoneLib;
using UnityEngine;

namespace PowerTools.Tools
{
    internal static class RagdollLegs
    {
        private static void Ragdoll()
        {
            Player.physicsRig.PhysicalLegs();
        }
        private static void Unragdoll()
        {
            Player.physicsRig.UnRagdollRig();
        }
        public static void BoneMenuCreator()
        {
            var ragdollLegs = Main.Category.CreateCategory("Ragdoll Legs", Color.magenta);
            ragdollLegs.CreateFunctionElement("Ragdoll Legs", "#00fc82", Ragdoll);
            ragdollLegs.CreateFunctionElement("Unragdoll Legs", "#00fc82", Unragdoll);
        }
    }
}