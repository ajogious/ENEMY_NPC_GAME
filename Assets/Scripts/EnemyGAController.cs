using UnityEngine;

public class EnemyGAController : MonoBehaviour
{
    public EnemyDNA currentDNA;

    [Header("Tracking Player")]
    public PlayerBehaviorTracker tracker;

    private void Start()
    {
        if (currentDNA == null)
        {
            currentDNA = ScriptableObject.CreateInstance<EnemyDNA>();
            currentDNA.aggression = 0.5f;
            currentDNA.dodgeChance = 0.5f;
            currentDNA.chaseRange = 5f;
        }
    }

    public void AdaptToPlayer()
    {
        if (tracker == null) return;

        if (tracker.attackCount >= 3 && tracker.moveDistance < 5)
        {
            currentDNA.aggression += 0.1f;
            currentDNA.dodgeChance += 0.1f;
        }
        else if (tracker.moveDistance > 5)
        {
            currentDNA.chaseRange += 2f;
        }

        currentDNA.Mutate();
    }

    public EnemyDNA GetCurrentDNA()
    {
        return currentDNA;
    }
}
