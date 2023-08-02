using BoneLib;
using BoneLib.BoneMenu;
using BoneLib.BoneMenu.Elements;
using MelonLoader;
using PowerTools.Tools;
using SLZ.VRMK;
using UnityEngine;

namespace PowerTools
{
    internal partial class Main : MelonMod
    {
        public static MenuCategory category;

        public static MelonPreferences_Category MelonPrefCategory { get; private set; }

        public override void OnEarlyInitializeMelon()
        {
            base.OnEarlyInitializeMelon();
        }

        public override void OnInitializeMelon()
        {
            BoneLib.Hooking.OnLevelInitialized += (_) => { OnSceneAwake(); };
            MelonPrefCategory = MelonPreferences.CreateCategory("Power Tools");
            DeathTimeCustomizer.MelonPreferencesCreator();
            ButtonDisabler.MelonPreferencesCreator();


            category = MenuManager.CreateCategory(
            "<color=#00ff72>P</color>" +
            "<color=#00ff80>o</color>" +
            "<color=#00ff8d>w</color>" +
            "<color=#00ff99>e</color>" +
            "<color=#00ffa5>r</color>" +
            "<color=#00ffb0> </color>" +
            "<color=#00ffba>T</color>" +
            "<color=#00ffc3>o</color>" +
            "<color=#00ffcc>o</color>" +
            "<color=#00ffd4>l</color>" +
            "<color=#00ffd4>s</color>"
                , Color.white);
            DeathTimeCustomizer.BonemenuCreator();
            ButtonDisabler.BonemenuCreator();
        }

        public void OnSceneAwake()
        {
            DeathTimeCustomizer.DeathTimeSetter();
            ButtonDisabler.DisableButtons();
        }

        public override void OnLateInitializeMelon()
        {
            base.OnLateInitializeMelon();
        }

        public override void OnUpdate()
        {

        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
        }
    }
}
