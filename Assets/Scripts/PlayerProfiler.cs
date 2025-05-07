using UnityEngine;

public class PlayerProfiler : MonoBehaviour
{
    public float aggressionScore = 0f;
    public float defensiveScore = 0f;

    public void RecordAggression()
    {
        aggressionScore += 1f;
    }

    public void RecordDefense()
    {
        defensiveScore += 1f;
    }

    public string GetPlayerStyle()
    {
        if (aggressionScore > defensiveScore + 3f)
            return "Aggressive";
        else if (defensiveScore > aggressionScore + 3f)
            return "Defensive";
        else
            return "Neutral";
    }
}
