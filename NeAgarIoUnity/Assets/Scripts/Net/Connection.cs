using Mirror;

/// <summary>
/// Класс для автоматического подключения игрока к серверу игры. Вызывается автоматически после авторизации.
/// </summary>
public class Connection : NetworkBehaviour
{
    /// <summary>
    /// Вызывается при старте сцены. Подключает игрока к серверу.
    /// </summary>
    private void Start()
    {
        System.Uri uri = new System.Uri("localhost:7777");

        NetworkManager nw = this.GetComponent<NetworkManager>();
        nw.StartClient(uri);
    }
}
