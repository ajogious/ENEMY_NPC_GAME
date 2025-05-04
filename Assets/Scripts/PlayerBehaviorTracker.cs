using UnityEngine;
using System.Collections;

public class PlayerBehaviorTracker : MonoBehaviour
{
    public float moveDistance;
    public int attackCount;

    private Vector3 lastPosition;
    public string currentPlayStyle = "Balanced";

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        // Track movement distance
        float distance = Vector3.Distance(transform.position, lastPosition);
        moveDistance += distance;
        lastPosition = transform.position;

        // Update playstyle on every frame for testing
        UpdatePlayStyle();
    }

    // Increment the attack count
    public void IncrementAttack()
    {
        attackCount++;
    }

    // Determine player's playstyle based on movement and attack patterns
    void UpdatePlayStyle()
    {
        Debug.Log(">>> Updating PlayStyle...");
        Debug.Log($"[Behavior] Attacks: {attackCount}, Distance: {moveDistance}");

        // Adjust behavior based on conditions
        if (attackCount >= 1 && moveDistance < 2f)
            currentPlayStyle = "Aggressive";
        else if (moveDistance > 5f && attackCount < 1)
            currentPlayStyle = "Stealthy";
        else
            currentPlayStyle = "Balanced";

        // Log the new playstyle
        Debug.Log("New Style: " + currentPlayStyle);
    }

    // Get the current playstyle
    public string GetPlayStyle()
    {
        return currentPlayStyle;
    }
}
