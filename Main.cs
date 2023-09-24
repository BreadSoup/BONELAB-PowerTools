using BoneLib;
using BoneLib.BoneMenu;
using BoneLib.BoneMenu.Elements;
using MelonLoader;
using PowerTools.Tools;
using UnityEngine;

namespace PowerTools
{
    internal partial class Main : MelonMod
    {
        public static MenuCategory Category;

        public static MelonPreferences_Category MelonPrefCategory { get; private set; }
        

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("Game path: " + Application.dataPath);
            Hooking.OnLevelInitialized += (_) => { OnSceneAwake(); };
            MelonPrefCategory = MelonPreferences.CreateCategory("Power Tools");
            DeathTimeCustomizer.MelonPreferencesCreator();
            ReloadOnDeathCustomizer.MelonPreferencesCreator();
            ButtonDisabler.MelonPreferencesCreator();
            RagdollOnDeath.MelonPreferencesCreator();
            VaultingToggle.MelonPreferencesCreator();


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
            Loadouts.BoneMenuCreator();
        }

        private static void OnSceneAwake()
        {
            DeathTimeCustomizer.DeathTimeSetter();
            
            ButtonDisabler.DisableButtons();

            ReloadOnDeathCustomizer.IsDefaultSet = false;
            ReloadOnDeathCustomizer.ReloadOnDeathSetter(ReloadOnDeathCustomizer.ReloadLevel);
            
            RagdollOnDeath.OnSetEnabled(RagdollOnDeath.RagdollOnDeathIsEnabled);
        }
        
        public override void OnUpdate()
        {
            //BugoSpray.BugoRemover();
        }


    }
}
