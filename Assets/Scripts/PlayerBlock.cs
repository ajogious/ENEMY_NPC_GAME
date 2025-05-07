using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    public float blockReductionFactor = 0.5f;
    public bool isBlocking = false;

    void Update()
    {
        isBlocking = Input.GetKey(KeyCode.B);
    }

    public float GetDamageReduction(float damage)
    {
        if (isBlocking)
        {
            Debug.Log("Blocked! Damage reduced.");
            return damage * blockReductionFactor;
        }
        return damage;
    }
}
