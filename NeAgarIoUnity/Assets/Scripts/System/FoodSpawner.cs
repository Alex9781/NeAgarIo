using UnityEngine;
using Mirror;

/// <summary>
/// Класс отвечающий за нахождение еды на сцене.
/// </summary>
public class FoodSpawner : NetworkBehaviour
{
    /// <summary>
    /// Префаб еды.
    /// </summary>
    [SerializeField] private GameObject Food;
    /// <summary>
    /// Ссылка на компонент в сцене в который будет складываться вся еда. 
    /// Необходим только для того, чтобы в окне иерархии было меньше объктов.
    /// </summary>
    [SerializeField] private GameObject Foods;
    /// <summary>
    /// Сколько одновременно будет находиться еды на сцене.
    /// </summary>
    [SerializeField] private int FoodCount;

    /// <summary>
    /// Рамки игрового поля, за которым не может появляться еда.
    /// </summary>
    private Vector2 SpawnRadius = GameGlobalSettings.GameField;

    /// <summary>
    /// При старте сервера спавнит максимальное количество еды.
    /// </summary>
    private void Start()
    {
        for (int i = 0; i < FoodCount; i++)
        {
            Spawn();
        }
    }

    /// <summary>
    /// Метод спавнит одну еду на сцене.
    /// </summary>
    public void SpawnFood()
    {
        Spawn();
    }
    
    /// <summary>
    /// Приватный метод для спавна еды.
    /// </summary>
    private void Spawn()
    {
        if (!isServer) { return; }

        Vector2 spawnPoint = new Vector2(
                Random.Range(-SpawnRadius.x, SpawnRadius.x),
                Random.Range(-SpawnRadius.y, SpawnRadius.y));

        GameObject spawned = Instantiate(Food, spawnPoint, Quaternion.identity);
        spawned.gameObject.transform.parent = Foods.transform;

        NetworkServer.Spawn(spawned);
    }
}
