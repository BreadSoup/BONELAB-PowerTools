using System;
using BoneLib;
using BoneLib.Nullables;
using MelonLoader;
using SLZ.Interaction;
using SLZ.Marrow.Data;
using SLZ.Marrow.Pool;
using SLZ.Marrow.Warehouse;
using UnityEngine;

namespace PowerTools.Tools
{
    public static class Loadouts
    {
        public static string HeadSlot;
        public static string BackLeftSlot;
        public static string BackRightSlot;
        public static string LeftHandSlot;
        public static string RightHandSlot;
        public static string Hipslot;

        private const string HeadSlotPath = "[PhysicsRig]/Head/HeadSlotContainer/WeaponReciever_01";
        private const string BackLeftSlotPath = "[PhysicsRig]/Chest/BackLf/ItemReciever";
        private const string BackRightSlotPath = "[PhysicsRig]/Chest/BackRt/ItemReciever";
        private const string LeftHandSlotPath = "[PhysicsRig]/Spine/SideLf/prop_handGunHolster/ItemReciever";
        private const string RightHandSlotPath = "[PhysicsRig]/Spine/SideRt/prop_handGunHolster/ItemReciever";
        private const string HipslotPath = "[PhysicsRig]/Spine/BackCt/ItemReciever";

        public static void BoneMenuCreator()
        {
            var loadouts = Main.Category.CreateCategory("Loadouts", "#00fc82");
            loadouts.CreateFunctionElement("Save Current Loadout", Color.green, delegate ()
            {
                SaveLoadout();
            }); 
            
            loadouts.CreateFunctionElement("Apply Test Loadout", Color.cyan, delegate ()
            {
                SpawnNimbusGun(HeadSlot, HeadSlotPath);
                SpawnNimbusGun(BackLeftSlot, BackLeftSlotPath);
                SpawnNimbusGun(BackRightSlot, BackRightSlotPath);
                SpawnNimbusGun(LeftHandSlot, LeftHandSlotPath);
                SpawnNimbusGun(RightHandSlot, RightHandSlotPath);
                SpawnNimbusGun(Hipslot, HipslotPath);
                
            }); 
           
        }

        private static void SaveLoadout()
        {
            var headSlot = GameObject.Find(HeadSlotPath);
            var backLeftSlot = GameObject.Find(BackLeftSlotPath);
            var backRightSlot = GameObject.Find(BackRightSlotPath);
            var leftHandSlot = GameObject.Find(LeftHandSlotPath);
            var rightHandSlot = GameObject.Find(RightHandSlotPath);
            var hipSlot = GameObject.Find(HipslotPath);
            
            
            GetBarcode(headSlot, ref HeadSlot);
            GetBarcode(backLeftSlot, ref BackLeftSlot);
            GetBarcode(backRightSlot, ref BackRightSlot);
            GetBarcode(leftHandSlot, ref LeftHandSlot);
            GetBarcode(rightHandSlot, ref RightHandSlot);
            GetBarcode(hipSlot, ref Hipslot);
        }
        
        private static void GetBarcode(GameObject slotVar , ref string slot)
        {
            if (slotVar.transform.childCount > 0)
            {
                slot = slotVar.GetComponentInChildren<AssetPoolee>().spawnableCrate._barcode._id;
            }
        }
        
        internal static void SpawnNimbusGun(string barcodeValue, string slotPath)
        {
            var slot = GameObject.Find(slotPath);
            var head = Player.playerHead.transform;
            
            var reference = new SpawnableCrateReference(barcodeValue);

            var spawnable = new Spawnable()
            {
                crateRef = reference
            };

            AssetSpawner.Register(spawnable);

            AssetSpawner.Spawn(spawnable, head.position + head.forward, default, new BoxedNullable<Vector3>(null),false, new BoxedNullable<int>(null), (Action<GameObject>)Action);
            return;

            void Action(GameObject go)
            {
                MelonLogger.Msg("Loaded object in holster with barcode ");
                slot.GetComponent<InventorySlotReceiver>().InsertInSlot(go.GetComponent<InteractableHost>());
                    
            }
        }
    }
}