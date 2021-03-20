using UnityEngine;

public class Weight : MonoBehaviour
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
