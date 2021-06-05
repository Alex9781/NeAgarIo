using UnityEngine;
using Mirror;

/// <summary>
/// ����� ���������� �� ���������� ��� �� �����.
/// </summary>
public class FoodSpawner : NetworkBehaviour
{
    /// <summary>
    /// ������ ���.
    /// </summary>
    [SerializeField] private GameObject Food;
    /// <summary>
    /// ������ �� ��������� � ����� � ������� ����� ������������ ��� ���. 
    /// ��������� ������ ��� ����, ����� � ���� �������� ���� ������ �������.
    /// </summary>
    [SerializeField] private GameObject Foods;
    /// <summary>
    /// ������� ������������ ����� ���������� ��� �� �����.
    /// </summary>
    [SerializeField] private int FoodCount;

    /// <summary>
    /// ����� �������� ����, �� ������� �� ����� ���������� ���.
    /// </summary>
    private Vector2 SpawnRadius = GameGlobalSettings.GameField;

    /// <summary>
    /// ��� ������ ������� ������� ������������ ���������� ���.
    /// </summary>
    private void Start()
    {
        for (int i = 0; i < FoodCount; i++)
        {
            Spawn();
        }
    }

    /// <summary>
    /// ����� ������� ��� �� �����.
    /// </summary>
    /// <param name="count">����������</param>
    /// <returns>������������ �� ���?</returns>
    public bool SpawnFood(int count)
    {
        try
        {
            for (int i = 0; i < count; i++)
            {
                Spawn();
            }
            return true;
        }
        catch (System.Exception)
        {
            return false;
        }

    }

    /// <summary>
    /// ��������� ����� ��� ������ ���.
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
