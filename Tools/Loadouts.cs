using System;
using BoneLib;
using BoneLib.BoneMenu.Elements;
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
        private static MenuCategory _loadouts;
        
        private static string _headSlot;
        private static string _backLeftSlot;
        private static string _backRightSlot;
        private static string _leftHandSlot;
        private static string _rightHandSlot;
        private static string _hipslot;

        private const string HeadSlotPath = "[PhysicsRig]/Head/HeadSlotContainer/WeaponReciever_01";
        private const string BackLeftSlotPath = "[PhysicsRig]/Chest/BackLf/ItemReciever";
        private const string BackRightSlotPath = "[PhysicsRig]/Chest/BackRt/ItemReciever";
        private const string LeftHandSlotPath = "[PhysicsRig]/Spine/SideLf/prop_handGunHolster/ItemReciever";
        private const string RightHandSlotPath = "[PhysicsRig]/Spine/SideRt/prop_handGunHolster/ItemReciever";
        private const string HipslotPath = "[PhysicsRig]/Spine/BackCt/ItemReciever";

        public static void BoneMenuCreator()
        {
            _loadouts = Main.Category.CreateCategory("Loadouts", "#00fc82");
            _loadouts.CreateFunctionElement("Save Current Loadout", Color.green, SaveLoadout); 
            
            _loadouts.CreateFunctionElement("Apply Test Loadout", Color.cyan, delegate
            {
                SpawnLoadout(_headSlot, HeadSlotPath);
                SpawnLoadout(_backLeftSlot, BackLeftSlotPath);
                SpawnLoadout(_backRightSlot, BackRightSlotPath);
                SpawnLoadout(_leftHandSlot, LeftHandSlotPath);
                SpawnLoadout(_rightHandSlot, RightHandSlotPath);
                SpawnLoadout(_hipslot, HipslotPath);
                
            }); 
           
        }

        private static int _loadoutNumber = 1;
        

        private static void SaveLoadout()
        {
            var headSlot = GameObject.Find(HeadSlotPath);
            var backLeftSlot = GameObject.Find(BackLeftSlotPath);
            var backRightSlot = GameObject.Find(BackRightSlotPath);
            var leftHandSlot = GameObject.Find(LeftHandSlotPath);
            var rightHandSlot = GameObject.Find(RightHandSlotPath);
            var hipSlot = GameObject.Find(HipslotPath);
            
            
            GetBarcode(headSlot, ref _headSlot);
            GetBarcode(backLeftSlot, ref _backLeftSlot);
            GetBarcode(backRightSlot, ref _backRightSlot);
            GetBarcode(leftHandSlot, ref _leftHandSlot);
            GetBarcode(rightHandSlot, ref _rightHandSlot);
            GetBarcode(hipSlot, ref _hipslot);
            
            BoneMenuLoadoutCreator();
        }
        
        private static void BoneMenuLoadoutCreator()
        {
            var headSlotLoadout = _headSlot;
            var backLeftSlotLoadout = _backLeftSlot;
            var backRightSlotLoadout = _backRightSlot;
            var leftHandSlotLoadout = _leftHandSlot;
            var rightHandSlotLoadout = _rightHandSlot;
            var hipslotLoadout = _hipslot;
            _loadoutNumber++;
            var loadout = _loadouts.CreateCategory("Loadout " + _loadoutNumber, Color.cyan);
            
            loadout.CreateFunctionElement("Apply Loadout", Color.green, delegate
            {
                SpawnLoadout(headSlotLoadout, HeadSlotPath);
                SpawnLoadout(backLeftSlotLoadout, BackLeftSlotPath);
                SpawnLoadout(backRightSlotLoadout, BackRightSlotPath);
                SpawnLoadout(leftHandSlotLoadout, LeftHandSlotPath);
                SpawnLoadout(rightHandSlotLoadout, RightHandSlotPath);
                SpawnLoadout(hipslotLoadout, HipslotPath);
            });
            /*Loadout.CreateFunctionElement("Delete", Color.red, delegate
            {
                _loadouts.RemoveElement(Loadout);
            });*/
        }
        
        private static void GetBarcode(GameObject slotVar , ref string slot)
        {
            if (slotVar.transform.childCount > 0)
            {
                slot = slotVar.GetComponentInChildren<AssetPoolee>().spawnableCrate._barcode._id;
            }
        }

        private static void SpawnLoadout(string barcodeValue, string slotPath)
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