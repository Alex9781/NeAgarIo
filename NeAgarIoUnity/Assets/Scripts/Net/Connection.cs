using Mirror;

public class Connection : NetworkBehaviour
{
    private void Start()
    {
        System.Uri uri = new System.Uri("localhost:7777");

        NetworkManager nw = this.GetComponent<NetworkManager>();
        nw.StartClient(uri);
    }
}
