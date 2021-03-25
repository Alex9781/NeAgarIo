using UnityEngine;

public class Player : MonoBehaviour
{
    public string PlayerId { get; private set; }
    public string PlayerName { get; private set; }

    private void Awake()
    {
        PlayerId = ExternalListener.PlayerId;
        PlayerName = ExternalListener.PlayerName;
    }
}
