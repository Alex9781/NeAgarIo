using System.Net.Sockets;
using System.Text;
using UnityEngine;

/// <summary>
/// �����, ����������� ��� ����� � �������� � �������� �� Qt.
/// </summary>
public class ExternalListener : MonoBehaviour
{
    /// <summary>
    /// Tcp ������ ��� ����� � ��������.
    /// </summary>
    public static TcpClient tcpClient;
    /// <summary>
    /// ����� ������ ��� �������� ����������� � ���� ������.
    /// </summary>
    public static string PlayerLogin;
    /// <summary>
    /// ������������ ��� ������.
    /// </summary>
    public static string PlayerName;

    private void Start()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        PlayerName = args[1];
        PlayerLogin = args[2];
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    /// <summary>
    /// �������� ����������� �� ������.
    /// </summary>
    /// <param name="results">��� ������ � ������ ������.</param>
    public static void SendResults(string results)
    {
        tcpClient = new TcpClient();
        tcpClient.Connect("localhost", 6547);
        tcpClient.ReceiveTimeout = -1;

        if (tcpClient.Connected)
        {
            byte[] _temp = Encoding.Unicode.GetBytes(" " + PlayerLogin + " " + results);

            NetworkStream ns = tcpClient.GetStream();
            ns.Write(_temp, 0, _temp.Length);
        }

        tcpClient.Close();
    }
}
