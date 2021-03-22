using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FoodSpawner : MonoBehaviour
{
    [SerializeField] private GameObject Food;
    [SerializeField] private int FoodCount;
    [SerializeField] private Color[] Colors;

    private Vector2 SpawnRadius = GameGlobalSettings.GameField;

    private void Start()
    {
        for (int i = 0; i < FoodCount; i++)
        {
            Vector2 spawnPoint = new Vector2(
                Random.Range(-SpawnRadius.x, SpawnRadius.x),
                Random.Range(-SpawnRadius.y, SpawnRadius.y));

            GameObject spawned = Instantiate(Food, spawnPoint, Quaternion.identity);
            spawned.GetComponent<SpriteRenderer>().color = Colors[Random.Range(0, Colors.Length)];
        }
    }
}
