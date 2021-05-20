using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ExternalListener : MonoBehaviour
{
    public static TcpClient tcpClient;
    public static string PlayerLogin;
    public static string PlayerName;

    private void Start()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        PlayerName = args[0];
        PlayerLogin = args[1];
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

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
