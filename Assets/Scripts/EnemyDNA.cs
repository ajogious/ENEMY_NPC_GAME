using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDNA", menuName = "GA/EnemyDNA")]
public class EnemyDNA : ScriptableObject
{
    public float aggression;  // Higher = attacks more often
    public float dodgeChance; // Higher = dodges more
    public float chaseRange;  // How far enemy will chase

    public void Mutate()
    {
        aggression = Mathf.Clamp01(aggression + Random.Range(-0.2f, 0.2f));
        dodgeChance = Mathf.Clamp01(dodgeChance + Random.Range(-0.2f, 0.2f));
        chaseRange = Mathf.Clamp(chaseRange + Random.Range(-1f, 1f), 2f, 15f);

    }

    public void Crossover(EnemyDNA partner)
    {
        aggression = (aggression + partner.aggression) / 2f;
        dodgeChance = (dodgeChance + partner.dodgeChance) / 2f;
        chaseRange = (chaseRange + partner.chaseRange) / 2f;
    }
}
