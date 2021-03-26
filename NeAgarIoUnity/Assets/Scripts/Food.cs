using UnityEngine;
using Mirror;

public class Food : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer FoodSprite;
    [SerializeField] private Color[] Colors;

    [SyncVar(hook = nameof(SetColor))]
    private Color FoodColor;

    public override void OnStartServer()
    {
        base.OnStartServer();

        FoodColor = Colors[Random.Range(0, Colors.Length)];
    }

    private void SetColor(Color _, Color newColor)
    {
        if (FoodSprite == null)
            FoodSprite = GetComponent<SpriteRenderer>();

        FoodSprite.color = newColor;
    }

    private void OnDestroy()
    {
        Destroy(FoodSprite);
    }
}
