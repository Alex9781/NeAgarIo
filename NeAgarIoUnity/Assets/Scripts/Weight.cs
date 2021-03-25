using UnityEngine;
using Mirror;

public class Weight : NetworkBehaviour
{
    [SerializeField] private float ObjectWeight;

    public float GetWeight()
    {
        return ObjectWeight;
    }

    public void AddWeight(float weight)
    {
        ObjectWeight += weight;
    }
}
