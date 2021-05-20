using UnityEngine;
using Mirror;

public class FoodSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject Food;
    [SerializeField] private GameObject Foods;
    [SerializeField] private int FoodCount;

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
        if (!isServer) { return; }

        Vector2 spawnPoint = new Vector2(
                Random.Range(-SpawnRadius.x, SpawnRadius.x),
                Random.Range(-SpawnRadius.y, SpawnRadius.y));

        GameObject spawned = Instantiate(Food, spawnPoint, Quaternion.identity);
        spawned.gameObject.transform.parent = Foods.transform;

        NetworkServer.Spawn(spawned);
    }
}
