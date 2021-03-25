using UnityEngine;
using Mirror;

[RequireComponent(typeof(Collider2D))]
public class FoodSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject Food;
    [SerializeField] private int FoodCount;
    [SerializeField] private Color[] Colors;
    [SerializeField] private GameObject Foods;

    private Vector2 SpawnRadius = GameGlobalSettings.GameField;

    private void Start()
    {
        for (int i = 0; i < FoodCount; i++)
        {
            Spawn();
        }
    }

    public void SpawnFoodOnEat()
    {
        Spawn();
    }

    private void Spawn()
    {
        Vector2 spawnPoint = new Vector2(
                Random.Range(-SpawnRadius.x, SpawnRadius.x),
                Random.Range(-SpawnRadius.y, SpawnRadius.y));

        GameObject spawned = Instantiate(Food, spawnPoint, Quaternion.identity);
        spawned.GetComponent<SpriteRenderer>().color = Colors[Random.Range(0, Colors.Length)];

        spawned.GetComponent<SpriteRenderer>().color = Colors[Random.Range(0, Colors.Length)];
        spawned.gameObject.transform.parent = Foods.transform;
    }
}
