using TMPro;
using UnityEngine;

public class PlayerStyleDisplay : MonoBehaviour
{
    public TextMeshProUGUI styleText;
    public PlayerBehaviorTracker tracker;

    void Update()
    {
        if (styleText != null && tracker != null)
        {
            string playStyle = tracker.GetPlayStyle();  // Get playstyle
            styleText.text = "Style: " + playStyle;
            Debug.Log("Updated UI with PlayStyle: " + playStyle);  // Debugging line
        }
    }

}
