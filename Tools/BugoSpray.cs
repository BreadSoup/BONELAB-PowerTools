using UnityEngine;
using TMPro;

namespace PowerTools.Tools
{
    public class BugoSpray
    {
        public static void BugoRemover()
        {
            TextMeshProUGUI[] textMeshProUGUIComponents = Object.FindObjectsOfType<TextMeshProUGUI>();
            TextMeshPro[] textMeshProComponents = Object.FindObjectsOfType<TextMeshPro>();

            // Iterate through TextMeshProUGUI components
            foreach (TextMeshProUGUI tmpText in textMeshProUGUIComponents)
            {
                TextMeshProUGUI the = tmpText.gameObject.GetComponent<TextMeshProUGUI>();
                if (the != null)
                {
                    if (the.m_text.Contains("BugoBug") || the.m_text.Contains("Budostayheadonarm") || the.m_text.Contains("Bugazzz06"))
                    {
                        // Replace the matched text with "BreadSoup"
                        the.m_text = the.m_text.Replace(the.m_text, "BreadSoup");
                        Debug.Log("TextMeshPro match found and replaced: " + the.m_text);
                    }
                }
            }
            
            foreach (TextMeshPro tmpText in textMeshProComponents)
            {
                TextMeshPro the = tmpText.gameObject.GetComponent<TextMeshPro>();
                if (the != null)
                {
                    if (the.text.Contains("BugoBug") || the.text.Contains("Budostayheadonarm") || the.text.Contains("Bugazzz06"))
                    {
                        the.text = the.text.Replace(the.text, "BreadSoup");
                        if (the.gameObject.active)
                        {
                            the.gameObject.SetActive(false);
                            the.gameObject.SetActive(true);
                        }
                        else
                        {
                            the.gameObject.SetActive(true);
                            the.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }
}