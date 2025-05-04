using TMPro;
using UnityEngine;

/// <summary>
/// Displays the current state of the enemy from the FSM onto a UI Text component.
/// </summary>
[RequireComponent(typeof(EnemyFSM))]
public class EnemyStateDisplay : MonoBehaviour
{
    [Tooltip("UI Text element to display the enemy's current FSM state.")]
    public TextMeshProUGUI stateText;

    private EnemyFSM fsm;

    private void Awake()
    {
        // Get the FSM component attached to the same GameObject
        fsm = GetComponent<EnemyFSM>();

        if (fsm == null)
            Debug.LogError("EnemyFSM component not found on " + gameObject.name);
    }

    private void Update()
    {
        // Ensure UI is assigned and FSM reference is valid
        if (fsm != null && stateText != null)
        {
            stateText.text = fsm.currentState.ToString();

            // Optional: Add color coding based on state for better UX
            stateText.color = GetColorForState(fsm.currentState);
        }
    }

    /// <summary>
    /// Returns a color based on the FSM state for visual feedback.
    /// </summary>
    private Color GetColorForState(EnemyFSM.EnemyState state)
    {
        return state switch
        {
            EnemyFSM.EnemyState.Patrolling => Color.white,
            EnemyFSM.EnemyState.Chasing => Color.yellow,
            EnemyFSM.EnemyState.Attacking => Color.red,
            EnemyFSM.EnemyState.Retreating => new Color(1f, 0.5f, 0f), // Orange
            EnemyFSM.EnemyState.Searching => Color.cyan,
            _ => Color.gray
        };
    }
}
