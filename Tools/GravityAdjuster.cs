using System;
using BoneLib;
using MelonLoader;
using UnityEngine;

namespace PowerTools.Tools
{
    public class GravityAdjuster
    {
        private static float _gravity = -9.81f;
        private static float _originalGravity = -9.8f;
        private static bool isEnabled;

        public static void BoneMenuCreator()
        {
            var gravityCustomizer = Main.Category.CreateCategory("Gravity Adjuster", "#00fc82");
        
            gravityCustomizer.CreateBoolElement("Mod Toggle", Color.yellow, isEnabled, OnSetEnabled);
            gravityCustomizer.CreateFloatElement("Gravity Value (0.1)", Color.yellow, _gravity, 0.1f, -100f, 100f, (r) =>
            {
                MelonPreferences.SetEntryValue("Power Tools", "Gravity Adjuster Value", r);
                MelonPreferences.Save();
                _gravity = r;
                GravityAdjust();
            });
            gravityCustomizer.CreateFloatElement("Gravity Value (1)", Color.yellow, _gravity, 1f, -100f, 100f, (r) =>
            {
                MelonPreferences.SetEntryValue("Power Tools", "Gravity Adjuster Value", r);
                MelonPreferences.Save();
                _gravity = r;
                GravityAdjust();
            });
            gravityCustomizer.CreateFloatElement("Gravity Value (10)", Color.yellow, _gravity, 10f, -100f, 100f, (r) =>
            {
                MelonPreferences.SetEntryValue("Power Tools", "Gravity Adjuster Value", r);
                MelonPreferences.Save();
                _gravity = r;
                GravityAdjust();
            });

            
            
            
                
        }

        private static void OnSetEnabled(bool value)
        {
            MelonPreferences.SetEntryValue("Power Tools", "Gravity Adjuster", value);
            MelonPreferences.Save();
            isEnabled = value;
            GravityAdjust();
        }
        

        public static void GravityAdjust()
        {
            if (isEnabled)
            {
                Physics.gravity = new Vector3(0, _gravity, 0);
            }
        }

        public static void MelonPreferencesCreator()
        {
            MelonPreferences.CreateEntry("Power Tools", "Gravity Adjuster", false);
            MelonPreferences.CreateEntry("Power Tools", "Gravity Adjuster Value", -9.81f);
        }
    }
}