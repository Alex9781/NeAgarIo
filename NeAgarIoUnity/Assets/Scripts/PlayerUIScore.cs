using UnityEngine;
using UnityEngine.UI;

public class PlayerUIScore : MonoBehaviour
{
    [SerializeField] private Text score;

    private void Update()
    {
        score.text = this.GetComponent<Weight>().GetWeight().ToString();
    }
}
