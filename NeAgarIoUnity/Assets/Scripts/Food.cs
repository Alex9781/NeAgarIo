using UnityEngine;
using Mirror;

/// <summary>
/// ����� ����������� ��������� ���. 
/// </summary>
public class Food : NetworkBehaviour
{
    /// <summary>
    /// ��������� ���������, ��������� ��� ����� ����� � ���.
    /// </summary>
    [SerializeField] private SpriteRenderer FoodSprite;
    /// <summary>
    /// ������ �� ������, � ������� ����� ����������� ���.
    /// </summary>
    [SerializeField] private Color[] Colors;

    /// <summary>
    /// ������� ���������� ��� ������������� ����� ��� � ���� �������.
    /// </summary>
    [SyncVar(hook = nameof(SetColor))]
    private Color FoodColor;

    /// <summary>
    /// ����� ����� ��� ��������� ���� ��� ������ �������. ���������� �������������.
    /// </summary>
    public override void OnStartServer()
    {
        base.OnStartServer();

        FoodColor = Colors[Random.Range(0, Colors.Length)];
    }

    /// <summary>
    /// ����� ���������������� ���� ��� � �������. ���������� �������������.
    /// </summary>
    /// <param name="_"></param>
    /// <param name="newColor"></param>
    private void SetColor(Color _, Color newColor)
    {
        if (FoodSprite == null)
            FoodSprite = GetComponent<SpriteRenderer>();

        FoodSprite.color = newColor;
    }

    /// <summary>
    /// ���������� ����� ��� �������. ������ ���, ����� ��� ������������ � ���� �������.
    /// </summary>
    private void OnDestroy()
    {
        Destroy(FoodSprite);
    }
}
