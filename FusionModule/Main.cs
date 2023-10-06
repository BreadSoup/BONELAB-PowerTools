using System;
using System.Collections;
using BoneLib;
using BoneLib.BoneMenu;
using BoneLib.BoneMenu.Elements;
using BoneLib.Nullables;
using LabFusion;
using LabFusion.Data;
using LabFusion.Network;
using LabFusion.Representation;
using MelonLoader;
using LabFusion.SDK.Modules;
using SLZ.Interaction;
using SLZ.Marrow.Data;
using SLZ.Marrow.Pool;
using SLZ.Marrow.Warehouse;
using SLZ.Props.Weapons;
using SLZ.Rig;
using UnityEngine;

namespace PowerToolsFusionModule
{
    internal abstract partial class Main : MelonMod
    {
        [HarmonyLib.HarmonyPatch(typeof(PowerTools.Tools.Loadouts), "FusionModuleSender")]
        public class FusionModuleSender : Module
        {
            public static FusionModuleSender Instance { get; private set; }
            
            public static void Prefix(string barcode, string slotPath)
            {
                if (NetworkInfo.IsClient)
                {
                    using (var writer = FusionWriter.Create())
                    {
                        using (var data = BasicStringData.CreateBarcode(barcode))
                        {
                            writer.Write(data);
                            using (var message = FusionMessage.ModuleCreate<BasicStringMessage>(writer))
                            {
                                MessageSender.SendToServer(NetworkChannel.Reliable, message);
                            }
                        }
                        using (var data = BasicStringData.CreateSlotPath(slotPath))
                        {
                            writer.Write(data);
                            using (var message = FusionMessage.ModuleCreate<BasicStringMessage>(writer))
                            {
                                MessageSender.SendToServer(NetworkChannel.Reliable, message);
                            }
                        }
                        
                        writer.Write(PlayerIdManager.LocalSmallId);
                        using (var message = FusionMessage.ModuleCreate<BasicStringMessage>(writer))
                        {
                            MessageSender.SendToServer(NetworkChannel.Reliable, message);
                        }
                    }
                }
            }
        }
        public class BasicStringData : IFusionSerializable, IDisposable
        {
            public string Barcode;

            public string SlotPath;

            public byte PlayerId;
            

            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }

            public void Serialize(FusionWriter writer)
            {
                writer.Write(Barcode);
                writer.Write(SlotPath);
                writer.Write(PlayerId);
                
            }

            public void Deserialize(FusionReader reader)
            {
                Barcode = reader.ReadString();
                SlotPath = reader.ReadString();
            }

            public static BasicStringData CreateBarcode(string message)
            {
                return new BasicStringData()
                {
                    Barcode = message,
                };
            }
            
            public static BasicStringData CreateSlotPath(string message)
            {
                return new BasicStringData()
                {
                    SlotPath = message,
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
                        if (data.Barcode != null && data.SlotPath != null && data.PlayerId != null)
                        {


                            // If this is handled by the socket server, and we are running the server, bounce it to all clients
                            // You can choose this way or other ways, but the behaviour is up to you!
                            if (NetworkInfo.IsServer && isServerHandled)
                            {
                                PlayerRep rep = null;
                                var slot = GameObject.Find(data.SlotPath);
                                
                                PlayerRepManager.TryGetPlayerRep(data.PlayerId, out rep);
                                RigManager rig = rep.RigReferences.RigManager;
                                var head = rig.physicsRig.m_head.transform;; //should work
                                
            
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
                
                                    slot.GetComponent<InventorySlotReceiver>().DespawnContents();
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
                            // Otherwise, we handle the message
                            else
                            {
                                

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

    }
}
