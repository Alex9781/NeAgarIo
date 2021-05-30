using UnityEngine;
using Mirror;

/// <summary>
/// Класс, описывающий общее поведение всех объектов которые можно съесть, будь то еда или игроки.
/// </summary>
public class Weight : NetworkBehaviour
{
    /// <summary>
    /// Текущий вес объекта.
    /// </summary>
    [SerializeField] private float ObjectWeight;

    /// <summary>
    /// Метод, возвращающий вес текущего объекта.
    /// </summary>
    /// <returns>Вес</returns>
    public float GetWeight()
    {
        return ObjectWeight;
    }

    /// <summary>
    /// Метод добавляющий к объекту массу.
    /// </summary>
    /// <param name="weight">Добавляемая масса.</param>
    public void AddWeight(float weight)
    {
        ObjectWeight += weight;
    }

    /// <summary>
    /// Сбрасывает вес объекта до 1.
    /// </summary>
    public void ResetWeight()
    {
        ObjectWeight = 1;
    }
}
