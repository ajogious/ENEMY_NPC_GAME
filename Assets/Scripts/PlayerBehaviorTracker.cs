using UnityEngine;

/// <summary>
/// Tracks player movement and attack patterns to determine playstyle.
/// </summary>
public class PlayerBehaviorTracker : MonoBehaviour
{
    [Header("Tracking Stats")]
    public float moveDistance = 0f;
    public int attackCount = 0;

    [Header("Current State")]
    public string currentPlayStyle = "Balanced";

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        TrackMovement();
        UpdatePlayStyle();
    }

    /// <summary>
    /// Increments the player's attack count.
    /// </summary>
    public void IncrementAttack()
    {
        attackCount++;
    }

    /// <summary>
    /// Tracks how far the player has moved since the last frame.
    /// </summary>
    private void TrackMovement()
    {
        float distance = Vector3.Distance(transform.position, lastPosition);
        moveDistance += distance;
        lastPosition = transform.position;
    }

    /// <summary>
    /// Updates the player's playstyle based on current movement and attacks.
    /// </summary>
    private void UpdatePlayStyle()
    {
        string newStyle;

        if (attackCount >= 1 && moveDistance < 2f)
            newStyle = "Aggressive";
        else if (moveDistance > 5f && attackCount < 1)
            newStyle = "Stealthy";
        else
            newStyle = "Balanced";

        if (newStyle != currentPlayStyle)
        {
            currentPlayStyle = newStyle;
            Debug.Log($"[Behavior] Attacks: {attackCount}, Distance: {moveDistance:F2}");
            Debug.Log(">>> New PlayStyle: " + currentPlayStyle);
        }
    }

    /// <summary>
    /// Returns the current playstyle of the player.
    /// </summary>
    public string GetPlayStyle()
    {
        return currentPlayStyle;
    }
}
