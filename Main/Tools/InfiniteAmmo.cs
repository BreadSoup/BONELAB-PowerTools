using BoneLib;
using HarmonyLib;
using MelonLoader;
using SLZ.Data;
using SLZ.Interaction;
using SLZ.Marrow.Data;
using SLZ.Player;
using SLZ.Props.Weapons;
using UnityEngine;

namespace PowerTools.Tools
{
    
    public abstract class InfiniteAmmo // this whole thing is so janky but if it works it works
    {
        
        private static MelonPreferences_Entry<bool> MelonPrefInfiniteAmmo { get; set; }
        private static bool InfiniteAmmoIsEnabled { get; set; }
        
        private static MelonPreferences_Entry<bool> MelonPrefGiveAmmoWhenEmpty { get; set; }

        private static bool GiveAmmoWhenEmpty { get; set; }
        
        private static MelonPreferences_Entry<bool> MelonPrefInfiniteMags { get; set; }

        private static bool InfiniteMags { get; set; }
        
        public static void MelonPreferencesCreator()
        {
            
            MelonPrefInfiniteAmmo = Main.MelonPrefCategory.CreateEntry("InfiniteAmmoIsEnabled", false);
            MelonPrefGiveAmmoWhenEmpty = Main.MelonPrefCategory.CreateEntry("GiveAmmoWhenEmpty", true);
            MelonPrefInfiniteMags = Main.MelonPrefCategory.CreateEntry("InfiniteMags", false);
            if (MelonPrefInfiniteAmmo != null)
            {
                InfiniteAmmoIsEnabled = MelonPrefInfiniteAmmo.Value;
            }
            if (MelonPrefGiveAmmoWhenEmpty != null)
            {
                GiveAmmoWhenEmpty = MelonPrefGiveAmmoWhenEmpty.Value;
            }
            if (MelonPrefInfiniteMags != null)
            {
                InfiniteMags = MelonPrefInfiniteMags.Value;
            }
        }
        public static void BoneMenuCreator()
        {
            var infiniteAmmo = Main.Category.CreateCategory("Infinite Ammo", "#bfff40");
            infiniteAmmo.CreateBoolElement("Mod Toggle", Color.yellow, InfiniteAmmoIsEnabled, OnSetEnabled);
            infiniteAmmo.CreateBoolElement("Give ammo when mag can't be full", "#ff3700", GiveAmmoWhenEmpty, OnGiveAmmoWhenEmpty);
            infiniteAmmo.CreateBoolElement("Infinite Mags", "#ff4640", InfiniteMags, OnInfiniteMags);
        }

        private static void OnSetEnabled(bool value)
        {
            MelonPrefInfiniteAmmo.Value = value;
            InfiniteAmmoIsEnabled = value;
            Main.MelonPrefCategory.SaveToFile(false);
        }
        
        private static void OnGiveAmmoWhenEmpty(bool value)
        {
            MelonPrefGiveAmmoWhenEmpty.Value = value;
            GiveAmmoWhenEmpty = value;
            Main.MelonPrefCategory.SaveToFile(false);
        }
        
        private static void OnInfiniteMags(bool value)
        {
            MelonPrefInfiniteMags.Value = value;
            InfiniteMags = value;
            Main.MelonPrefCategory.SaveToFile(false);
        }
        
        

        [HarmonyPatch(typeof(AmmoInventory), "RemoveCartridge")]
        public class TaxReturn
        {
            private static void Prefix(AmmoInventory __instance, CartridgeData cartridge, int count)
            {
                if (InfiniteAmmoIsEnabled)
                {
                    __instance.AddCartridge(cartridge, count);
                }
            }
        }

        [HarmonyPatch(typeof(InventoryAmmoReceiver), "OnHandGrab")]
        public class AmmoCheckPatch
        {
            public static void Prefix()
            {
                if (InfiniteAmmoIsEnabled && GiveAmmoWhenEmpty)
                {
                    var light = Player.rigManager.AmmoInventory.GetCartridgeCount("light");
                    if (light <= 0)
                    {
                        Bankruptcy(Player.rigManager.AmmoInventory.lightAmmoGroup);
                    }

                    var medium = Player.rigManager.AmmoInventory.GetCartridgeCount("medium");
                    if (medium <= 0)
                    {
                        Bankruptcy(Player.rigManager.AmmoInventory.mediumAmmoGroup);
                    }

                    var heavy = Player.rigManager.AmmoInventory.GetCartridgeCount("heavy");
                    if (heavy <= 25)
                    {
                        Player.rigManager.AmmoInventory.AddCartridge(Player.rigManager.AmmoInventory.heavyAmmoGroup,
                            25); //shotguns are supper weird so this is the best I can do
                    }
                }
            }

            private static void Bankruptcy(AmmoGroup group)
            {
                Player.rigManager.AmmoInventory.AddCartridge(group, 1);
                MelonLogger.Msg("added 1 ammo to " + group.name);
            }
        }

        [HarmonyPatch(typeof(Magazine), "OnGrab")]
        public class CashBack
        {
                public static void Postfix()
                {
                    if (InfiniteAmmoIsEnabled && GiveAmmoWhenEmpty)
                    {
                        MelonLogger.Msg("postfix ran");
                        var leftMag = Player.GetComponentInHand<Magazine>(Player.leftHand);
                        var rightMag = Player.GetComponentInHand<Magazine>(Player.rightHand);
                        if (leftMag != null)
                        {
                            MelonLogger.Msg("left mag");
                            int leftMagMax = leftMag.magazineState.magazineData.rounds;
                            int leftCartridgeCount = Player.rigManager.AmmoInventory.GetCartridgeCount(leftMag.magazineState.cartridgeData);
                            if (leftCartridgeCount < leftMagMax)
                            {
                                BankStatement(leftMag, leftMagMax, leftCartridgeCount);
                            }
                        }

                        if (rightMag != null)
                        {
                            MelonLogger.Msg("right mag");
                            int rightMagMax = rightMag.magazineState.magazineData.rounds;
                            int rightCartridgeCount =
                                Player.rigManager.AmmoInventory.GetCartridgeCount(rightMag.magazineState.cartridgeData);
                            if (rightCartridgeCount < rightMagMax)
                            {
                                BankStatement(rightMag, rightMagMax, rightCartridgeCount);
                            }
                        }
                    }
                }
                private static void BankStatement(Magazine mag, int magMax, int cartridgeCount)
                    {
                        if (Player.rigManager.AmmoInventory.GetCartridgeCount("light") == cartridgeCount)
                        {
                            Loan(mag, Player.rigManager.AmmoInventory.lightAmmoGroup, magMax);
                        }
                        else if (Player.rigManager.AmmoInventory.GetCartridgeCount("medium") == cartridgeCount)
                        {
                            Loan(mag, Player.rigManager.AmmoInventory.mediumAmmoGroup, magMax);
                        }
                        else if (Player.rigManager.AmmoInventory.GetCartridgeCount("heavy") == cartridgeCount)
                        {
                            Loan(mag, Player.rigManager.AmmoInventory.heavyAmmoGroup, magMax);
                        }

                    }

                    private static void Loan(Magazine mag, AmmoGroup group, int amount)
                    {
                        if (Player.rigManager.AmmoInventory.GetCartridgeCount(group.KeyName) == 1)
                        {
                            Player.rigManager.AmmoInventory.AddCartridge(group, -1);
                        }
                        Player.rigManager.AmmoInventory.AddCartridge(group, amount);
                        mag.magazineState.Refill();
                    }
        }

        [HarmonyPatch(typeof(Gun), "OnFire")]
        public class CreditCard
        {
            public static void Postfix(Gun __instance)
            {
                if (InfiniteAmmoIsEnabled && InfiniteMags)
                {
                    __instance.gameObject.GetComponentInChildren<Magazine>().magazineState.Refill();
                }
            }
            
        }
    }
}
