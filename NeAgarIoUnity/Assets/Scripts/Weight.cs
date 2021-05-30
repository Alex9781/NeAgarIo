using UnityEngine;
using Mirror;

/// <summary>
/// �����, ����������� ����� ��������� ���� �������� ������� ����� ������, ���� �� ��� ��� ������.
/// </summary>
public class Weight : NetworkBehaviour
{
    /// <summary>
    /// ������� ��� �������.
    /// </summary>
    [SerializeField] private float ObjectWeight;

    /// <summary>
    /// �����, ������������ ��� �������� �������.
    /// </summary>
    /// <returns>���</returns>
    public float GetWeight()
    {
        return ObjectWeight;
    }

    /// <summary>
    /// ����� ����������� � ������� �����.
    /// </summary>
    /// <param name="weight">����������� �����.</param>
    public void AddWeight(float weight)
    {
        ObjectWeight += weight;
    }

    /// <summary>
    /// ���������� ��� ������� �� 1.
    /// </summary>
    public void ResetWeight()
    {
        ObjectWeight = 1;
    }
}
