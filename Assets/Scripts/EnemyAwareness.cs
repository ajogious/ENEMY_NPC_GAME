using UnityEngine;
using TMPro;

public class EnemyAwareness : MonoBehaviour
{
    public enum AwarenessState { Unaware, Suspicious, Alerted, Engaged }
    public AwarenessState currentState = AwarenessState.Unaware;

    public TextMeshProUGUI stateText;
    public EnemyFOV fov;

    private float timeInSight = 0f;
    private float suspiciousTime = 1.5f;
    private float alertedTime = 3f;
    private float loseSightDelay = 2f;
    private float timeSinceLastSeen = 0f;

    void Update()
    {
        if (fov.canSeePlayer)
        {
            timeInSight += Time.deltaTime;
            timeSinceLastSeen = 0f;

            if (timeInSight >= alertedTime)
                currentState = AwarenessState.Engaged;
            else if (timeInSight >= suspiciousTime)
                currentState = AwarenessState.Alerted;
            else
                currentState = AwarenessState.Suspicious;
        }
        else
        {
            timeSinceLastSeen += Time.deltaTime;

            if (timeSinceLastSeen >= loseSightDelay)
            {
                currentState = AwarenessState.Unaware;
                timeInSight = 0f;
            }
        }

        if (stateText != null)
        {
            stateText.text = currentState.ToString();

            // Optional: change color based on state
            switch (currentState)
            {
                case AwarenessState.Unaware:
                    stateText.color = Color.white;
                    break;
                case AwarenessState.Suspicious:
                    stateText.color = Color.yellow;
                    break;
                case AwarenessState.Alerted:
                    stateText.color = new Color(1f, 0.5f, 0f); // orange
                    break;
                case AwarenessState.Engaged:
                    stateText.color = Color.red;
                    break;
            }
        }


    }
}
