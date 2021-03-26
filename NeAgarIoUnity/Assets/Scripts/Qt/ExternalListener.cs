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
        tcpClient.Connect("localhost", 69696);
        tcpClient.ReceiveTimeout = -1;

        if (tcpClient.Connected)
        {
            byte[] _temp = new byte[tcpClient.ReceiveBufferSize];

            NetworkStream ns = tcpClient.GetStream();
            ns.Read(_temp, 0, tcpClient.ReceiveBufferSize);

            string temp = Encoding.Unicode.GetString(_temp);
            PlayerId = temp.Substring(0, temp.IndexOf(" "));
            PlayerName = temp.Substring(temp.IndexOf(" ") + 1, temp.Length - 1 - temp.IndexOf(" "));

            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }
}
