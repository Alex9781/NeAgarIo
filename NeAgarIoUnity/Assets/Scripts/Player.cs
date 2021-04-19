using UnityEngine;
using Mirror;
using System.Collections;

public class Player : NetworkBehaviour
{
    public string PlayerId { get; private set; }
    public string PlayerName { get; private set; }

    [Header("Dependencies")]
    [SerializeField] private Rigidbody2D PlayerRigidbody2D = null;
    [SerializeField] private Camera PlayerCamera = null;
    [SerializeField] private Weight Weight = null;
    [SerializeField] private TextMesh playerNameText = null;
    [SerializeField] private GameObject DeathCanvas = null;

    [Header("Settings")]
    [SerializeField] private float Speed = 1;

    private Vector2 AllowedRadius = GameGlobalSettings.GameField;
    private float cameraSizeOffset;
    private bool isAlive = true;

    [SyncVar(hook = nameof(OnNameChanged))]
    private string playerName;
    [SyncVar(hook = nameof(OnCompStateChanged))]
    private bool compState = true;

    private void OnNameChanged(string _Old, string _New)
    {
        playerNameText.text = playerName;
    }

    private void OnCompStateChanged(bool _Old, bool _New)
    {
        this.GetComponent<Collider2D>().enabled = compState;
        this.GetComponent<SpriteRenderer>().enabled = compState;
        playerNameText.GetComponent<MeshRenderer>().enabled = compState;
    }

    [Command]
    public void CmdSetupPlayer(string _name)
    {
        playerName = _name;
    }

    [Command]
    public void CmdChangeCompState(bool b)
    {
        compState = b;
    }

    //[Command]
    //public void CmdChangePlayerPosition()
    //{
    //    this.transform.position = new Vector3(
    //        Random.Range(-GameGlobalSettings.GameField.x, GameGlobalSettings.GameField.x),
    //        Random.Range(-GameGlobalSettings.GameField.y, GameGlobalSettings.GameField.y),
    //        0);
    //}


    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        PlayerId = ExternalListener.PlayerId;
        PlayerName = ExternalListener.PlayerName;

        PlayerCamera.gameObject.SetActive(true);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        PlayerCamera = Camera.main;
        PlayerCamera.transform.SetParent(this.transform);
        PlayerCamera.transform.localPosition = new Vector3(0, 0, -10);

        cameraSizeOffset = PlayerCamera.orthographicSize;

        playerName = ExternalListener.PlayerName;
        CmdSetupPlayer(playerName);
        playerNameText.text = playerName;

        //CmdChangePlayerPosition();
    }


    private void FixedUpdate()
    {
        if (!hasAuthority) { return; }
        if (isAlive)
        {
            //Vector2 cursorPos = PlayerCamera.ScreenToWorldPoint(Input.mousePosition);
            //Vector2 vectorDelta = cursorPos - new Vector2(
            //    this.gameObject.transform.position.x,
            //    this.gameObject.transform.position.y);

            //PlayerRigidbody2D.velocity = new Vector2(
            //    Mathf.Clamp(vectorDelta.x, -Speed, Speed),
            //    Mathf.Clamp(vectorDelta.y, -Speed, Speed)).normalized * Speed;

            this.transform.position = new Vector3(
                Mathf.Clamp(this.transform.position.x, -AllowedRadius.x, AllowedRadius.x),
                Mathf.Clamp(this.transform.position.y, -AllowedRadius.y, AllowedRadius.y),
                0);

            Vector2 v2 = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            PlayerRigidbody2D.velocity = v2.normalized * Speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Weight>(out Weight _w))
        {
            if (Weight.GetWeight() > _w.GetWeight())
            {
                Weight.AddWeight(_w.GetWeight() / 10);
                StopAllCoroutines();
                StartCoroutine(ChangeScale());

                if (collision.TryGetComponent(out Player _p))
                {
                    _p.Death();
                }
                else
                {
                    Destroy(collision.gameObject);
                }

                if (collision.TryGetComponent(out Food _))
                {
                    FoodSpawner f = FindObjectOfType<FoodSpawner>();
                    f.SpawnFoodOnEat();
                }
            }
        }
    }

    private IEnumerator ChangeScale()
    {
        float currentScale = this.transform.localScale.x;
        float currentCameraSize = PlayerCamera.orthographicSize;
        float scaleInterpolation;
        float cameraScaleInterpolation;

        for (float t = 0; t <= 1f; t += Time.deltaTime)
        {
            scaleInterpolation = Mathf.Lerp(currentScale, Weight.GetWeight(), t);
            this.transform.localScale = new Vector3(scaleInterpolation, scaleInterpolation, 1);

            cameraScaleInterpolation = Mathf.Lerp(currentCameraSize, Weight.GetWeight() + cameraSizeOffset - 1f, t);
            PlayerCamera.orthographicSize = cameraScaleInterpolation;

            yield return null;
        }
    }


    public void Death()
    {
        isAlive = false;

        PlayerRigidbody2D.velocity = Vector3.zero;
        CmdChangeCompState(false);

        if (hasAuthority)
            DeathCanvas.SetActive(true);

        ExternalListener.SendResults(PlayerId + " " + this.GetComponent<Weight>().GetWeight());
    }

    public void Retry()
    {
        isAlive = true;

        //CmdChangePlayerPosition();
        CmdChangeCompState(true);
        
        DeathCanvas.SetActive(false);

        Weight.ResetWeight();
        this.gameObject.transform.localScale = Vector3.one;
    }
}
