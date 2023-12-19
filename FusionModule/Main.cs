using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using BoneLib;
using BoneLib.BoneMenu;
using BoneLib.BoneMenu.Elements;
using BoneLib.Nullables;
using LabFusion;
using LabFusion.Data;
using LabFusion.Network;
using LabFusion.Representation;
using LabFusion.SDK.Modules;
using MelonLoader;
using SLZ.Interaction;
using SLZ.Marrow.Data;
using SLZ.Marrow.Pool;
using SLZ.Marrow.Warehouse;
using SLZ.Props.Weapons;
using SLZ.Rig;
using UnityEngine;
using Module = LabFusion.SDK.Modules.Module;


namespace PowerToolsFusionModule
{

    internal partial class Main : MelonMod
    {

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("Loading module");
            ModuleHandler.LoadModule(System.Reflection.Assembly.GetExecutingAssembly());
        }
    }

    public class LoadoutsModule : Module
        {
            public override void OnModuleLoaded()
            {
                MelonLogger.Msg("Loaded PowerTools Module!");
            }
        }

        [HarmonyLib.HarmonyPatch(typeof(PowerTools.Tools.Loadouts), "FusionModuleSender")]
        public class FusionModuleSender
        {
            public static FusionModuleSender Instance { get; private set; }
            
            public static void Prefix(string barcode, string slotPath)
            {
                MelonLogger.Msg("prefix");
                if (NetworkInfo.IsClient)
                {
                    MelonLogger.Msg("is client");
                    MelonLogger.Msg(barcode);
                    MelonLogger.Msg(slotPath);
                    using (var writer = FusionWriter.Create())
                    {
                        MelonLogger.Msg("breakpoint 1");
                        using (var data = BasicStringData.Create(barcode, slotPath))
                        {
                            MelonLogger.Msg("breakpoint 2");
                            writer.Write(data);
                            MelonLogger.Msg("breakpoint 3");
                            using (var message = FusionMessage.ModuleCreate<BasicStringMessage>(writer))
                            {
                                MelonLogger.Msg("sending message");
                                MessageSender.SendToServer(NetworkChannel.Reliable, message);
                                MelonLogger.Msg("message sent");
                            }                
                        }
                    }
                }
            }
        }
        public class BasicStringData : IFusionSerializable, IDisposable
        {
            public string Barcode;

            public string SlotPath;

            public byte PlayerID;
            
            

            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }

            public void Serialize(FusionWriter writer)
            {
                writer.Write(Barcode);
                writer.Write(SlotPath);
                
            }

            public void Deserialize(FusionReader reader)
            {
                Barcode = reader.ReadString();
                SlotPath = reader.ReadString();
            }
            
            
            public static BasicStringData Create(string slotMessage, string barcodeMessage)
            {
                MelonLogger.Msg(PlayerIdManager.LocalSmallId);
                return new BasicStringData()
                {
                    SlotPath = slotMessage,
                    Barcode = barcodeMessage,
                    PlayerID = PlayerIdManager.LocalSmallId
                };
            }
            
            
        }

        public class BasicStringMessage : ModuleMessageHandler 
        {
            public override void HandleMessage(byte[] bytes, bool isServerHandled = false)
            {
                using (var reader = FusionReader.Create(bytes))
                {
                    using (var data = reader.ReadFusionSerializable<BasicStringData>())
                    {
                        MelonLogger.Msg("message recived doing check");
                        if (data.Barcode != null && data.SlotPath != null)
                        {
                            MelonLogger.Msg("check passed");

                            // If this is handled by the socket server, and we are running the server, bounce it to all clients
                            // You can choose this way or other ways, but the behaviour is up to you!
                            if (NetworkInfo.IsServer)
                            {
                                PlayerRep rep = null;
                                
                                PlayerRepManager.TryGetPlayerRep(data.PlayerID, out rep);
                                RigManager rig = rep.RigReferences.RigManager;
                                var head = rig.physicsRig.m_head.transform;; //should work
                                
                                
                                var slot = SlotGetter.GetSlot(data.SlotPath, rig);
                                
                                var reference = new SpawnableCrateReference(data.Barcode);

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
                
                                    slot.DespawnContents();
                                    MelonLogger.Msg("Loaded object in holster with barcode ");
                                    var gun /*Genius variable rename I know*/ = go.GetComponent<Gun>(); //Thanks Swipez for some of the code used in this method
                                    if (gun != null)
                                    {
                                        gun.InstantLoad();
                                        gun.CeaseFire();
                                        gun.Charge();
                                        MelonCoroutines.Start(WaitAndFixGun(gun));
                                    }
                                    slot.InsertInSlot(go.GetComponent<InteractableHost>());
                                    MelonLogger.Msg("Loaded object in holster");
                    
                                }
                            } 
                        }
                    }
                }
            }
            private static IEnumerator WaitAndFixGun(Gun gun) //Thanks Swipez again
            {
                yield return null;
                yield return null;
                yield return null;
                gun.CompleteSlidePull();
                gun.CompleteSlideReturn();
            }
        }

        public class SlotGetter //im so good at naming things
        {
        public static InventorySlotReceiver GetSlot(string slotName, RigManager rig)
        {
            if (slotName == "[PhysicsRig]/Head/HeadSlotContainer/WeaponReciever_01")
            {
                return rig.physicsRig.m_head.gameObject.GetComponentInChildren<InventorySlotReceiver>();
            }

            if (slotName == "[PhysicsRig]/Chest/BackLf/ItemReciever")
            {
                return rig.physicsRig.m_chest.transform.FindChild("BackLf").gameObject
                    .GetComponentInChildren<InventorySlotReceiver>();
            }

            if (slotName == "[PhysicsRig]/Chest/BackRt/ItemReciever")
            {
                return rig.physicsRig.m_chest.transform.FindChild("BackRt").gameObject
                    .GetComponentInChildren<InventorySlotReceiver>();
            }

            if (slotName == "[PhysicsRig]/Spine/SideLf/ItemReciever")
            {
                return rig.physicsRig.m_spine.FindChild("SideLf").gameObject
                    .GetComponentInChildren<InventorySlotReceiver>();
            }

            if (slotName == "[PhysicsRig]/Spine/SideRt/ItemReciever")
            {
                return rig.physicsRig.m_spine.FindChild("SideRt").gameObject
                    .GetComponentInChildren<InventorySlotReceiver>();
            }

            if (slotName == "[PhysicsRig]/Spine/BackCt/ItemReciever")
            {
                return rig.physicsRig.m_spine.FindChild("BackCt").gameObject
                    .GetComponentInChildren<InventorySlotReceiver>();
            }

            return null;
        }
        }
}
