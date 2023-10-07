using System;
using System.Collections;
using System.IO;
using System.Linq;
using BoneLib;
using BoneLib.BoneMenu;
using BoneLib.BoneMenu.Elements;
using BoneLib.Nullables;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using SLZ.Interaction;
using SLZ.Marrow.Data;
using SLZ.Marrow.Pool;
using SLZ.Marrow.Warehouse;
using SLZ.Props.Weapons;
using SLZ.Rig;
using UnityEngine;
using Object = Il2CppSystem.Object;

namespace PowerTools.Tools
{
    public static class Loadouts
    {
        private static MenuCategory _loadouts;

        private static readonly string LoadoutPath = Path.Combine(Main.PowerToolsPath, "Loadouts");


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

        //TODO: Check if it works for custom maps I think the path is different on custom maps
        private const string HeadSlotPath = "[PhysicsRig]/Head/HeadSlotContainer/WeaponReciever_01";
        private const string BackLeftSlotPath = "[PhysicsRig]/Chest/BackLf/ItemReciever";
        private const string BackRightSlotPath = "[PhysicsRig]/Chest/BackRt/ItemReciever";
        private const string LeftHandSlotPath = "[PhysicsRig]/Spine/SideLf/prop_handGunHolster/ItemReciever";
        private const string RightHandSlotPath = "[PhysicsRig]/Spine/SideRt/prop_handGunHolster/ItemReciever";
        private const string HipslotPath = "[PhysicsRig]/Spine/BackCt/ItemReciever";

        public static void BoneMenuCreator()
        {
            if (!Directory.Exists(LoadoutPath))
                Directory.CreateDirectory(LoadoutPath);
            _loadouts = Main.Category.CreateCategory("Loadouts", "#ff4060");
            _loadouts.CreateFunctionElement("Save Current Loadout", Color.green, SaveLoadout);

            // Loading loadouts. This is very basic and idiotic and should probably be redone if you want.
            string[] files = Directory.GetFiles(LoadoutPath);
            if (files.Length > 0)
            {
                foreach (string file in files)
                {
                    string[] data = File.ReadAllLines(file);

                    var loadout = _loadouts.CreateCategory(data[0], Color.cyan);

                    loadout.CreateFunctionElement("Apply Loadout", Color.green, delegate
                    {
                        if (data[1] != "")
                            SpawnLoadout(data[1], HeadSlotPath);
                        if (data[2] != "")
                            SpawnLoadout(data[2], BackLeftSlotPath);
                        if (data[3] != "")
                            SpawnLoadout(data[3], BackRightSlotPath);
                        if (data[4] != "")
                            SpawnLoadout(data[4], LeftHandSlotPath);
                        if (data[5] != "")
                            SpawnLoadout(data[5], RightHandSlotPath);
                        if (data[6] != "")
                            SpawnLoadout(data[6], HipslotPath);
                    });

                    Color betterRed = new Color(0.9607843f, 0.258823543f, 0.258823543f);
                    loadout.CreateFunctionElement("Delete Loadout", betterRed, delegate
                    {
                        _loadouts.Elements.Remove(loadout);
                        File.Delete(Path.Combine(LoadoutPath, loadout.Name + ".txt"));
                        MenuManager.SelectCategory(_loadouts);

                    }, "Are you sure?");

                    if (data[7] != "")
                        BoneMenuNameButtonCreator("Head slot: ", data[7], loadout);
                    if (data[8] != "")
                        BoneMenuNameButtonCreator("Back left slot: ", data[8], loadout);
                    if (data[9] != "")
                        BoneMenuNameButtonCreator("Back right slot: ", data[9], loadout);
                    if (data[10] != "")
                        BoneMenuNameButtonCreator("Left handgun slot: ", data[10], loadout);
                    if (data[11] != "")
                        BoneMenuNameButtonCreator("Right handgun slot: ", data[11], loadout);
                    if (data[12] != "")
                        BoneMenuNameButtonCreator("Hip slot: ", data[12], loadout);
                }
            }
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
            if (!Directory.Exists(LoadoutPath))
                Directory.CreateDirectory(LoadoutPath);

            var headSlotLoadout = _headSlot;
            var backLeftSlotLoadout = _backLeftSlot;
            var backRightSlotLoadout = _backRightSlot;
            var leftHandSlotLoadout = _leftHandSlot;
            var rightHandSlotLoadout = _rightHandSlot;
            var hipslotLoadout = _hipslot;

            // Making sure that _loadoutNumber is always above the file with the max _loadoutNumber. Whenver renaming is implemented this will be a struggle to fix (maybe).
            var fileNames = Directory.GetFiles(LoadoutPath);
            if (fileNames.Length > 0)
            {
                _loadoutNumber = fileNames.Count() + 1;
            } else
            {
                _loadoutNumber = 1;
            }


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
            
            Color betterRed = new Color(0.9607843f, 0.258823543f, 0.258823543f);
            loadout.CreateFunctionElement("Delete Loadout", betterRed, delegate
            {
                _loadouts.Elements.Remove(loadout);
                File.Delete(Path.Combine(LoadoutPath, loadout.Name + ".txt"));
                MenuManager.SelectCategory(_loadouts);
                
            }, "Are you sure?");

            BoneMenuNameButtonCreator("Head slot: ", _headSlotName, loadout);
            BoneMenuNameButtonCreator("Back left slot: ", _backLeftSlotName, loadout);
            BoneMenuNameButtonCreator("Back right slot: ", _backRightSlotName, loadout);
            BoneMenuNameButtonCreator("Left handgun slot: ", _leftHandSlotName, loadout);
            BoneMenuNameButtonCreator("Right handgun slot: ", _rightHandSlotName, loadout);
            BoneMenuNameButtonCreator("Hip slot: ", _hipSlotName, loadout);

            string[] loadoutData = new string[]
            {
                loadout.Name, 
                _headSlot, 
                _backLeftSlot, 
                _backRightSlot, 
                _leftHandSlot, 
                _rightHandSlot, 
                _hipslot, 
                _headSlotName,
                _backLeftSlotName,
                _backRightSlotName,
                _leftHandSlotName,
                _rightHandSlotName,
                _hipSlotName
            };
            
            File.WriteAllLines(Path.Combine(LoadoutPath, loadout.Name + ".txt"), loadoutData);
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
            MelonLogger.Msg(slot.name);
            MelonLogger.Msg(Player.physicsRig.m_head.GetComponentInChildren<InventorySlotReceiver>());
            
            FusionModuleSender(barcodeValue, slotPath);
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
                //TODO: if on a fusion server make drop weapon destroy the weapon instead of dropping it or just dont spawn weapon into holster if there is a weapon in it
                
                slot.GetComponent<InventorySlotReceiver>().DropWeapon();
                MelonLogger.Msg("Loaded object in holster with barcode ");
                var gun /*Genius variable rename I know*/ = go.GetComponent<Gun>(); //Thanks Swipez for some of the code used in this method
                if (gun != null)
                {
                    gun.InstantLoad();
                    gun.CeaseFire();
                    gun.Charge();
                    MelonCoroutines.Start(WaitAndFixGun(gun));
                }
                slot.GetComponent<InventorySlotReceiver>().InsertInSlot(go.GetComponent<InteractableHost>());
                    
            }
        }

        public static void FusionModuleSender(string barcode, string slotPath)
        {
            
        }
        
        private static IEnumerator WaitAndFixGun(Gun gun) //Thanks Swipez again
        {
            yield return null;
            yield return null;
            yield return null;
            gun.CompleteSlidePull();
            gun.CompleteSlideReturn();
        }
        
        private static void BoneMenuNameButtonCreator(string info, string name, MenuCategory loadout)
        {
            if (name != null)
            {
                loadout.CreateFunctionElement(info + name, Color.white, delegate{});
            }
        }
    }
    public class LoadoutData : Il2CppSystem.Object
    {
        public string Name { get; set; }

        public string HeadHolster { get; set; }
        public string BackLeftHolster { get; set; }
        public string BackRightHolster { get; set; }
        public string LeftHandHolster { get; set; }
        public string RightHandHolster { get; set; }
        public string HipHolster { get; set; }

        public string HeadBarcode { get; set; }
        public string BackLeftBarcode { get; set; }
        public string BackRightBarcode { get; set; }
        public string LeftHandBarcode { get; set; }
        public string RightHandBarcode { get; set; }
        public string HipBarcode { get; set; }

    }
}