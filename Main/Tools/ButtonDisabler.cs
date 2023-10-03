using MelonLoader;
using UnityEngine;

namespace PowerTools.Tools
{
    internal static class ButtonDisabler
    {
        private static bool _endOfLevelButton;

        private static MelonPreferences_Entry<bool> MelonPrefEnabled { get;  set; }
        private static bool ButtonDisablerIsEnabled { get; set; }
        private static MelonPreferences_Entry<bool> MelonPrefEndOfLevelButton { get; set; }

        public static void MelonPreferencesCreator()
        {
            MelonPrefEnabled = Main.MelonPrefCategory.CreateEntry("ButtonDisablerIsEnabled", false);
            ButtonDisablerIsEnabled = MelonPrefEnabled.Value;
            MelonPrefEndOfLevelButton = Main.MelonPrefCategory.CreateEntry("Disable end of level button", false);

            if (MelonPrefEndOfLevelButton != null)
            {
                _endOfLevelButton = MelonPrefEndOfLevelButton.Value;
            }
        }

        public static void BoneMenuCreator()
        {
            var deathTimeCustomizer = Main.Category.CreateCategory("Button Disabler ", Color.yellow);

            deathTimeCustomizer.CreateBoolElement("Mod Toggle", Color.yellow, ButtonDisablerIsEnabled, OnSetEnabled);

            deathTimeCustomizer.CreateBoolElement("Disable Next Level Button", "#ff9900", _endOfLevelButton, OnEndOfLevelButtonEnabled);
        }
        public static void DisableButtons()
        {
            var objectsWithKeyword = Object.FindObjectsOfType<Transform>(true);
            foreach (Transform obj in objectsWithKeyword)
            {
                if (obj.name.Contains("FLOORS") || obj.name.Contains("LoadButtons") || obj.name.Contains("prop_bigButton") || obj.name.Contains("INTERACTION"))
                {

                    for (int i = 0; i < obj.childCount; i++)
                    {
                        Transform child = obj.GetChild(i);
                        SLZ.Interaction.ButtonToggle buttonToggle = child.GetComponent<SLZ.Interaction.ButtonToggle>();
                        if (buttonToggle != null && ButtonDisablerIsEnabled)
                        {
                            if (_endOfLevelButton)
                            {
                                buttonToggle.enabled = false;
                            }
                            else if (!_endOfLevelButton)
                            {
                                if (!child.name.Contains("prop_bigButton_NEXTLEVEL"))
                                {
                                    buttonToggle.enabled = false;
                                }
                                if (child.name.Contains("prop_bigButton_NEXTLEVEL"))
                                {
                                    buttonToggle.enabled = true;
                                }
                            }
                        }
                        else if (buttonToggle != null && !ButtonDisablerIsEnabled)
                        {
                            buttonToggle.enabled = true;
                        }
                    }
                }
            }
        }

        private static void OnSetEnabled(bool value)
        {
            ButtonDisablerIsEnabled = value;
            MelonPrefEnabled.Value = value;
            Main.MelonPrefCategory.SaveToFile(false);
            DisableButtons();
        }

        private static void OnEndOfLevelButtonEnabled(bool value)
        {
            _endOfLevelButton = value;
            MelonPrefEndOfLevelButton.Value = value;
            Main.MelonPrefCategory.SaveToFile(false);
            DisableButtons();
        }
    }
}
