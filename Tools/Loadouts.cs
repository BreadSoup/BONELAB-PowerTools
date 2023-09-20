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
        private static GameObject _slotsParent;
        private static Transform _slots;
        private static GameObject _nimbus;


        public static void SlotFinder()
        {
             _slotsParent = GameObject.Find("INVENTORYSLOTS");
             if (_slotsParent != null)
             {
                 _slots = _slotsParent.transform.Find("scaleOffset");
             }
        }

        /* public static void LoadoutSetter()
        {
            if (_slots != null)
            {
                //this isn't confusing at all!

                GameObject HeadSlot = GameObject.Find("/[RigManager (Blank)]/[PhysicsRig]/Head/HeadSlotContainer/WeaponReciever_01");
                GameObject BackLeftSlot = GameObject.Find("/[PhysicsRig]/Chest/BackLf/ItemReciever");
                /*var slot3 = _slots.transform.GetChild(2).gameObject;
                var slot4 = _slots.transform.GetChild(3).gameObject;
                var slot6 = _slots.transform.GetChild(5).gameObject;*//*

                SpawnNimbusGun();
                
                if (_nimbus != null)
                {
                    MelonLogger.Msg(_nimbus.name);
                    InteractableHost nimbusHost = _nimbus.GetComponent<InteractableHost>();
                    BackLeftSlot.GetComponent<InventorySlotReceiver>().InsertInSlot(nimbusHost);
                }
                else
                {
                    MelonLogger.Msg(":(");
                }
            }


        }*/
    

        internal static void SpawnNimbusGun()
        {
            var backLeftSlot = GameObject.Find("[PhysicsRig]/Chest/BackLf/ItemReciever");
            var head = Player.playerHead.transform;

            const string barcode = "c1534c5a-6b38-438a-a324-d7e147616467";
            var reference = new SpawnableCrateReference(barcode);

            var spawnable = new Spawnable()
            {
                crateRef = reference
            };

            AssetSpawner.Register(spawnable);

            AssetSpawner.Spawn(spawnable, head.position + head.forward, default, new BoxedNullable<Vector3>(null),false, new BoxedNullable<int>(null), (Action<GameObject>)Action);
            return;

            void Action(GameObject go)
            {
                if (backLeftSlot == null)
                {
                    MelonLogger.Msg("BackLeftSlot is null");
                }
                else
                {
                    if (go == null)
                    {
                        MelonLogger.Msg("go is null");
                    }
                    else
                    {
                        MelonLogger.Msg("Loaded object in holster with barcode ");
                        backLeftSlot.GetComponent<InventorySlotReceiver>().InsertInSlot(go.GetComponent<InteractableHost>());
                    }
                }
            }
        }
    }
}