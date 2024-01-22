using BoneLib;
using MelonLoader;
using SLZ.Rig;
using UnityEngine;

namespace PowerTools.Tools
{
    public class BrickShooter
    {
        public static void dfgijkhokjnhgrshjikhjiusgihuseg()
        {
            if (Player.rigManager == null)
            {
                Debug.Log("Player.rigManager is null");
                return;
            }

            if (Player.rigManager.gameObject.transform == null)
            {
                Debug.Log("Player.rigManager.gameObject is null");
                return;
            }

            Camera headObject = Player.rigManager.GetComponentInChildren<Camera>();

            if (headObject == null)
            {
                Debug.Log("No Camera component found in the children of Player.rigManager's gameObject");
                return;
            }

            if (headObject.gameObject.name != "Head")
            {
                Debug.Log("The Camera component found is not named 'Head'");
                return;
            }
            headObject.gameObject.transform.rotation = headObject.gameObject.transform.rotation * Quaternion.Euler(-1, 1, 1);
            var transform = headObject.transform;
            Vector3 scale = transform.localScale;
            scale.x = -1;
            transform.localScale = scale;
            
        }//todo: kill
        // WHAT DID I MEAN BY THAT????????????? WHY DID I MAKE THIS???? WHY IS IT IN BRICK SHOOTER THIS CHANGES LEFT EYE TO RIGHT EYE AND VICE VERSA THIS IS AWFUL
    }
}