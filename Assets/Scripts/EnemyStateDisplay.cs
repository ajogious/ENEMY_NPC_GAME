using TMPro;
using UnityEngine;

public class EnemyStateDisplay : MonoBehaviour
{
    public TextMeshProUGUI stateText;
    private EnemyFSM fsm; // Your existing FSM script

    void Start()
    {
        fsm = GetComponent<EnemyFSM>(); // Adjust name if yours is different
    }

    void Update()
    {
        if (fsm != null && stateText != null)
        {
            stateText.text = fsm.currentState.ToString(); // Or fsm.GetCurrentStateName()
        }
    }
}
