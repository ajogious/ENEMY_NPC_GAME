using UnityEngine;
using TMPro;

public class EnemyAwareness : MonoBehaviour
{
    public enum AwarenessState { Unaware, Suspicious, Alerted, Engaged }
    public AwarenessState currentState = AwarenessState.Unaware;

    [Header("UI & Detection References")]
    public TextMeshProUGUI stateText;
    public EnemyFOV fov;

    [Header("References")]
    public EnemyFSM enemyFSM;  // Drag this in Inspector or assign in Start()


    [Header("Timings (seconds)")]
    [SerializeField] private float suspiciousTime = 1.5f;
    [SerializeField] private float alertedTime = 3f;
    [SerializeField] private float loseSightDelay = 2f;

    private float timeInSight = 0f;
    private float timeSinceLastSeen = 0f;

    private void Update()
    {
        UpdateAwarenessState();
        UpdateStateUI();
    }

    /// <summary>
    /// Updates the awareness state based on whether the enemy can see the player.
    /// </summary>
    private void UpdateAwarenessState()
    {
        if (fov.canSeePlayer)
        {
            HandlePlayerSeen();
        }
        else
        {
            HandlePlayerLost();
        }
    }

    /// <summary>
    /// Handles logic when the player is within the enemy's field of view.
    /// </summary>
    private void HandlePlayerSeen()
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

    /// <summary>
    /// Handles logic when the player is no longer visible to the enemy.
    /// </summary>
    private void HandlePlayerLost()
    {
        timeSinceLastSeen += Time.deltaTime;

        if (timeSinceLastSeen >= loseSightDelay)
        {
            currentState = AwarenessState.Unaware;
            timeInSight = 0f;
        }
    }

    /// <summary>
    /// Updates the UI text and color to reflect the current awareness state.
    /// </summary>
    private void UpdateStateUI()
    {
        if (stateText == null) return;

        stateText.text = currentState.ToString();
        stateText.color = GetStateColor(currentState);
    }

    /// <summary>
    /// Returns a color based on the awareness state.
    /// </summary>
    private Color GetStateColor(AwarenessState state)
    {
        switch (state)
        {
            case AwarenessState.Unaware: return Color.white;
            case AwarenessState.Suspicious: return Color.yellow;
            case AwarenessState.Alerted: return new Color(1f, 0.5f, 0f); // orange
            case AwarenessState.Engaged: return Color.red;
            default: return Color.gray;
        }
    }
}
