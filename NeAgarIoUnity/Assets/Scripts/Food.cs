using UnityEngine;
using Mirror;

/// <summary>
/// Класс описывающий поведение еды. 
/// </summary>
public class Food : NetworkBehaviour
{
    /// <summary>
    /// Компонент отрисовки, необходим для смены цвета у еды.
    /// </summary>
    [SerializeField] private SpriteRenderer FoodSprite;
    /// <summary>
    /// Массив из цветов, в которые может покраситься еда.
    /// </summary>
    [SerializeField] private Color[] Colors;

    /// <summary>
    /// Сетевая переменная для синхронизации цвета еды у всех игроков.
    /// </summary>
    [SyncVar(hook = nameof(SetColor))]
    private Color FoodColor;

    /// <summary>
    /// Метод задаёт еду случайный цвет при старте сервера. Вызывается автоматически.
    /// </summary>
    public override void OnStartServer()
    {
        base.OnStartServer();

        FoodColor = Colors[Random.Range(0, Colors.Length)];
    }

    /// <summary>
    /// Метод синхронизирующий цвет еды у игроков. Вызывается автоматически.
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
    /// Вызывается когда еду съедают. Делает так, чтобы еда уничтожалась у всех игроков.
    /// </summary>
    private void OnDestroy()
    {
        Destroy(FoodSprite);
    }
}
