using Mirror;

/// <summary>
/// ����� ��� ��������������� ����������� ������ � ������� ����. ���������� ������������� ����� �����������.
/// </summary>
public class Connection : NetworkBehaviour
{
    /// <summary>
    /// ���������� ��� ������ �����. ���������� ������ � �������.
    /// </summary>
    private void Start()
    {
        System.Uri uri = new System.Uri("localhost:7777");

        NetworkManager nw = this.GetComponent<NetworkManager>();
        nw.StartClient(uri);
    }
}
