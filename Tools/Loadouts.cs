﻿using System;
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
        
        private static string _headSlotName;
        private static string _backLeftSlotName;
        private static string _backRightSlotName;
        private static string _leftHandSlotName;
        private static string _rightHandSlotName;
        private static string _hipSlotName;

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
        }

        private static int _loadoutNumber;
        
        private static void SaveLoadout()
        {
            var headSlot = GameObject.Find(HeadSlotPath);
            var backLeftSlot = GameObject.Find(BackLeftSlotPath);
            var backRightSlot = GameObject.Find(BackRightSlotPath);
            var leftHandSlot = GameObject.Find(LeftHandSlotPath);
            var rightHandSlot = GameObject.Find(RightHandSlotPath);
            var hipSlot = GameObject.Find(HipslotPath);
            
            
            
            GetBarcode(headSlot, ref _headSlot, ref _headSlotName);
            GetBarcode(backLeftSlot, ref _backLeftSlot, ref _backLeftSlotName);
            GetBarcode(backRightSlot, ref _backRightSlot, ref _backRightSlotName);
            GetBarcode(leftHandSlot, ref _leftHandSlot, ref _leftHandSlotName);
            GetBarcode(rightHandSlot, ref _rightHandSlot, ref _rightHandSlotName);
            GetBarcode(hipSlot, ref _hipslot, ref _hipSlotName);
            
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
            
            
            BoneMenuNameButtonCreator( "Head slot: ", _headSlotName, loadout);
            BoneMenuNameButtonCreator("Back left slot: ", _backLeftSlotName, loadout);
            BoneMenuNameButtonCreator("Back right slot: ", _backRightSlotName, loadout);
            BoneMenuNameButtonCreator("Left handgun slot: ", _leftHandSlotName, loadout);
            BoneMenuNameButtonCreator("Right handgun slot: ", _rightHandSlotName, loadout);
            BoneMenuNameButtonCreator("Hip slot: ", _hipSlotName, loadout);

        }
        
        private static void GetBarcode(GameObject slotVar , ref string slot, ref string name)
        {
            if (slotVar.transform.childCount > 0)
            {
                slot = slotVar.GetComponentInChildren<AssetPoolee>().spawnableCrate._barcode._id;
                name = slotVar.GetComponentInChildren<AssetPoolee>().spawnableCrate.name;
            }
            else
            {
                name = null;
                slot = null;
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

        private static void BoneMenuNameButtonCreator(string info, string name, MenuCategory loadout)
        {
            if (name != null)
            {
                loadout.CreateFunctionElement(info + name, Color.white, delegate{});
            }
        }
    }
}