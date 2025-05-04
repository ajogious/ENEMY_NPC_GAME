using TMPro;
using UnityEngine;

public class PlayerStyleDisplay : MonoBehaviour
{
    public TextMeshProUGUI styleText;
    public PlayerBehaviorTracker tracker;

    private string lastStyle = "";

    void Start()
    {
        if (styleText == null) Debug.LogWarning("StyleText is not assigned.");
        if (tracker == null) Debug.LogWarning("PlayerBehaviorTracker is not assigned.");
    }

    void Update()
    {
        if (styleText != null && tracker != null)
        {
            string currentStyle = tracker.GetPlayStyle();
            if (currentStyle != lastStyle)
            {
                styleText.text = "Style: " + currentStyle;
                Debug.Log("Updated UI with PlayStyle: " + currentStyle);
                lastStyle = currentStyle;
            }
        }
    }
}
