using BoneLib;
using BoneLib.BoneMenu;
using BoneLib.BoneMenu.Elements;
using MelonLoader;
using PowerTools.Tools;
using System.IO;
using UnityEngine;

namespace PowerTools
{
    internal partial class Main : MelonMod
    {
        public static MenuCategory Category;

        public static MelonPreferences_Category MelonPrefCategory { get; private set; }

        public static readonly string PowerToolsPath = Path.Combine(MelonUtils.UserDataDirectory, "PowerTools");

        public override void OnInitializeMelon()
        {
            Hooking.OnLevelInitialized += (_) => { OnSceneAwake(); };
            MelonPrefCategory = MelonPreferences.CreateCategory("Power Tools");
            
            DeathTimeCustomizer.MelonPreferencesCreator();
            
            ReloadOnDeathCustomizer.MelonPreferencesCreator();
            
            ButtonDisabler.MelonPreferencesCreator();
            
            RagdollOnDeath.MelonPreferencesCreator();
            
            VaultingToggle.MelonPreferencesCreator();
            
            //Loadouts.MelonPreferencesCreator(); removed
            
            GravityAdjuster.MelonPreferencesCreator();
            
            InfiniteAmmo.MelonPreferencesCreator();


            Category = MenuManager.CreateCategory(
                "<color=#00FF72>P</color>" +
                "<color=#00FF80>o</color>" +
                "<color=#00FF8D>w</color>" +
                "<color=#00FF99>e</color>" +
                "<color=#00FFA5>r</color>" +
                "<color=#00FFB0> </color>" +
                "<color=#00FFBA>T</color>" +
                "<color=#00FFC3>o</color>" +
                "<color=#00FFCC>o</color>" +
                "<color=#00FFD4>l</color>" +
                "<color=#00FFD4>s</color>",
                Color.white);
            DeathTimeCustomizer.BoneMenuCreator();
            
            ReloadOnDeathCustomizer.BoneMenuCreator();
            
            ButtonDisabler.BoneMenuCreator();
            
            RagdollOnDeath.BoneMenuCreator();
            
            VaultingToggle.BoneMenuCreator();
            
            GravityAdjuster.BoneMenuCreator();
            
            //Loadouts.BoneMenuCreator();
            
            InfiniteAmmo.BoneMenuCreator();
            
            RagdollLegs.BoneMenuCreator();
            
        }

        private static void OnSceneAwake()
        {
            DeathTimeCustomizer.DeathTimeSetter();
            
            ButtonDisabler.DisableButtons();

            ReloadOnDeathCustomizer.IsDefaultSet = false;
            ReloadOnDeathCustomizer.ReloadOnDeathSetter(ReloadOnDeathCustomizer.ReloadLevel);
            
            RagdollOnDeath.OnSetEnabled(RagdollOnDeath.RagdollOnDeathIsEnabled);
            
            GravityAdjuster.GravityAdjust();
        }
        
        public override void OnUpdate() // Only run tools under OnUpdate() if theres no alternatives and if an alternative is found please PR it
        {
            //BugoSpray.BugoRemover(); this is stupid I only made it for a bit
            
            //BrickShooter.dfgijkhokjnhgrshjikhjiusgihuseg(); IT DOESNT EVEN SHOOT BRICKS??????????? WHY
        }


    }
}
