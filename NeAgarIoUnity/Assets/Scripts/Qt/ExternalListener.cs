using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ExternalListener : MonoBehaviour
{
    public static TcpClient tcpClient;
    public static string PlayerId;
    public static string PlayerName;

    private void Start()
    {
        tcpClient = new TcpClient();
        tcpClient.Connect("localhost", 6969);
        tcpClient.ReceiveTimeout = -1;

        if (tcpClient.Connected)
        {
            byte[] _temp = new byte[tcpClient.ReceiveBufferSize];

            NetworkStream ns = tcpClient.GetStream();
            ns.Read(_temp, 0, tcpClient.ReceiveBufferSize);

            string temp = Encoding.Unicode.GetString(_temp);
            PlayerId = temp.Substring(0, temp.IndexOf(" "));
            PlayerName = temp.Substring(temp.IndexOf(" ") + 1, temp.Length - 1 - temp.IndexOf(" "));

            print(PlayerId + PlayerName);

            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }

    public static void SendResults(string results)
    {
        if (tcpClient.Connected)
        {
            byte[] _temp = Encoding.Unicode.GetBytes(results);

            NetworkStream ns = tcpClient.GetStream();
            ns.Write(_temp, 0, _temp.Length);
        }
    }
}
