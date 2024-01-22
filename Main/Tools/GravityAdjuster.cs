using BoneLib.BoneMenu.Elements;
using MelonLoader;
using UnityEngine;
// ReSharper disable AccessToModifiedClosure

namespace PowerTools.Tools
{
    public abstract class GravityAdjuster
    {
        private static MelonPreferences_Entry<float> MelonPrefGravityValue { get; set; }
        private static float _gravity = -9.81f;
        //private static float _originalGravity = -9.8f;
        private static bool _isEnabled;
        
        public static void MelonPreferencesCreator()
        {
            MelonPrefGravityValue = Main.MelonPrefCategory.CreateEntry("Gravity Adjuster Value", 9.81f);
            if (MelonPrefGravityValue != null)
            {
                _gravity = MelonPrefGravityValue.Value;
            }
        }
        
        public static void BoneMenuCreator()
        {
            var gravityCustomizer = Main.Category.CreateCategory("Gravity Adjuster", "#4555ed");
        
            gravityCustomizer.CreateBoolElement("Mod Toggle", Color.yellow, _isEnabled, OnSetEnabled);

            //100% a better way to do this but I don't feel like doing it
            FloatElement one = null;
            FloatElement ten = null;
            
            var pointOne = gravityCustomizer.CreateFloatElement("Gravity Value (0.1)", "#cc51fc", _gravity, 0.1f, -25f, 25f, (r) =>
            {
                MelonPrefGravityValue.Value = r;
                Main.MelonPrefCategory.SaveToFile(false);
                _gravity = r;
                one?.SetValue(r);
                ten?.SetValue(r);
                GravityAdjust();
            });
            one = gravityCustomizer.CreateFloatElement("Gravity Value (1)", "#cc51fc", _gravity, 1f, -25f, 25f, (r) =>
            {
                MelonPrefGravityValue.Value = r;
                Main.MelonPrefCategory.SaveToFile(false);
                pointOne?.SetValue(r);
                ten?.SetValue(r);
                _gravity = r;
                GravityAdjust();
            });
            ten = gravityCustomizer.CreateFloatElement("Gravity Value (5)", "#cc51fc", _gravity, 5f, -25f, 25f, (r) =>
            {
                MelonPrefGravityValue.Value = r;
                Main.MelonPrefCategory.SaveToFile(false);
                pointOne?.SetValue(r);
                one?.SetValue(r);
                _gravity = r;
                GravityAdjust();
            });
            
        }

        private static void OnSetEnabled(bool value)
        {
            _isEnabled = value;
            if (value)
            {
                GravityAdjust();
            }
            else
            {
                GravityReset();
            }
        }

        private static void GravityReset()
        {
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }


        public static void GravityAdjust()
        {
            if (_isEnabled)
            {
                Physics.gravity = new Vector3(0, _gravity, 0);
            }
        }


    }
}