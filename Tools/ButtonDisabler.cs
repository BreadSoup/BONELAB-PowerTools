using Il2CppSystem;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PowerTools.Tools
{
    internal class ButtonDisabler
    {
        public static bool EndOfLevelButton;

        public static MelonPreferences_Entry<bool> MelonPrefEnabled { get; private set; }
        public static bool ButtonDisablerIsEnabled { get; private set; }
        public static MelonPreferences_Entry<bool> MelonPrefEndOfLevelButton { get; private set; }

        public static void MelonPreferencesCreator()
        {
            MelonPrefEnabled = Main.MelonPrefCategory.CreateEntry<bool>("ButtonDisablerIsEnabled", false, null, null, false, false, null, null);
            ButtonDisablerIsEnabled = MelonPrefEnabled.Value;
            MelonPrefEndOfLevelButton = Main.MelonPrefCategory.CreateEntry<bool>("Disable end of level button", false, null, null, false, false, null, null);

            if (MelonPrefEndOfLevelButton != null)
            {
                EndOfLevelButton = MelonPrefEndOfLevelButton.Value;
            }
        }

        public static void BonemenuCreator()
        {
            var DeathTimeCustomizer = Main.category.CreateCategory("Button Disabler ", Color.yellow);

            DeathTimeCustomizer.CreateBoolElement("Mod Toggle", Color.yellow, ButtonDisablerIsEnabled, new System.Action<bool>(OnSetEnabled));

            DeathTimeCustomizer.CreateBoolElement("Disable Next Level Button", "#ff9900", EndOfLevelButton, new System.Action<bool>(OnEndOfLevelButtonEnabled));
        }
        public static void DisableButtons()
        {
            var objectsWithKeyword = Transform.FindObjectsOfType<Transform>(true);
            foreach (Transform obj in objectsWithKeyword)
            {
                if (obj.name.Contains("FLOORS") || obj.name.Contains("LoadButtons") || obj.name.Contains("prop_bigButton") || obj.name.Contains("INTERACTION"))
                {

                    for (int i = 0; i < obj.childCount; i++)
                    {
                        Transform child = obj.GetChild(i);
                        SLZ.Interaction.ButtonToggle ButtonToggle = child.GetComponent<SLZ.Interaction.ButtonToggle>();
                        if (ButtonToggle != null && ButtonDisablerIsEnabled)
                        {
                            if (EndOfLevelButton)
                            {
                                ButtonToggle.enabled = false;
                            }
                            else if (!EndOfLevelButton)
                            {
                                if (!child.name.Contains("prop_bigButton_NEXTLEVEL"))
                                {
                                    ButtonToggle.enabled = false;
                                }
                                if (child.name.Contains("prop_bigButton_NEXTLEVEL"))
                                {
                                    ButtonToggle.enabled = true;
                                }
                            }
                        }
                        else if (ButtonToggle != null && !ButtonDisablerIsEnabled)
                        {
                            ButtonToggle.enabled = true;
                        }
                    }
                }
            }
        }

        public static void OnSetEnabled(bool value)
        {
            ButtonDisablerIsEnabled = value;
            MelonPrefEnabled.Value = value;
            Main.MelonPrefCategory.SaveToFile(false);
            DisableButtons();
        }
        public static void OnEndOfLevelButtonEnabled(bool value)
        {
            EndOfLevelButton = value;
            MelonPrefEndOfLevelButton.Value = value;
            Main.MelonPrefCategory.SaveToFile(false);
            DisableButtons();
        }
    }
}
