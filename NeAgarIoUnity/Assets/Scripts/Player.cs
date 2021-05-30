using UnityEngine;
using Mirror;
using System.Collections;

/// <summary>
/// �����, ���������� �� ���������� ������� � �������������� ������ � ������� ���������.
/// </summary>
public class Player : NetworkBehaviour
{
    /// <summary>
    /// ������������ ��� ������.
    /// </summary>
    public string PlayerName { get; private set; }

    /// <summary>
    /// ��������� ������ ������ ��� ������������.
    /// </summary>
    [Header("Dependencies")]
    [SerializeField] private Rigidbody2D PlayerRigidbody2D = null;
    /// <summary>
    /// ��������� ������ ������.
    /// </summary>
    [SerializeField] private Camera PlayerCamera = null;
    /// <summary>
    /// ��������� ���� ������.
    /// </summary>
    [SerializeField] private Weight Weight = null;
    /// <summary>
    /// ��������� UI ��� ����������� ����� ������.
    /// </summary>
    [SerializeField] private TextMesh playerNameText = null;
    /// <summary>
    /// ��������� UI ��� ����������� ����� ������.
    /// </summary>
    [SerializeField] private GameObject DeathCanvas = null;

    /// <summary>
    /// ������� �������� ������.
    /// </summary>
    [Header("Settings")]
    [SerializeField] private float Speed = 1;

    /// <summary>
    /// ����� �� ������� ����� �� ����� �����.
    /// </summary>
    private Vector2 AllowedRadius = GameGlobalSettings.GameField;
    /// <summary>
    /// �������� ��������� ������ �� ������ � ������ ������ ����. 
    /// ��������� ��� ������������ ��������� ������ ��� ���������� �������� ������.
    /// </summary>
    private float cameraSizeOffset;
    /// <summary>
    /// ���� ��� ��������� ��� �� ����� � ����������� ������ � ������ � ���������� �� ����������.
    /// </summary>
    private bool isAlive = true;

    /// <summary>
    /// ������� ���������� ����� ������.
    /// </summary>
    [SyncVar(hook = nameof(OnNameChanged))]
    private string playerName;
    /// <summary>
    /// ���������� �������� ��������� ������. ��� ��� ����.
    /// </summary>
    [SyncVar(hook = nameof(OnCompStateChanged))]
    private bool compState = true;
    
    /// <summary>
    /// ����� ���������������� ����� � �������. ���������� �������������.
    /// </summary>
    /// <param name="_Old"></param>
    /// <param name="_New"></param>
    private void OnNameChanged(string _Old, string _New)
    {
        playerNameText.text = playerName;
    }

    /// <summary>
    /// ����� ���������������� ��������� �������. ���������� �������������.
    /// </summary>
    /// <param name="_Old"></param>
    /// <param name="_New"></param>
    private void OnCompStateChanged(bool _Old, bool _New)
    {
        this.GetComponent<Collider2D>().enabled = compState;
        this.GetComponent<SpriteRenderer>().enabled = compState;
        playerNameText.GetComponent<MeshRenderer>().enabled = compState;
    }

    /// <summary>
    /// ������� ������� �� ������������ ������ �����.
    /// </summary>
    /// <param name="_name">��� ������</param>
    [Command]
    public void CmdSetupPlayer(string _name)
    {
        playerName = _name;
    }

    /// <summary>
    /// ������� �������� �� ����� ��������� ������.
    /// </summary>
    /// <param name="b">���?</param>
    [Command]
    public void CmdChangeCompState(bool b)
    {
        compState = b;
    }

    /// <summary>
    /// ���������� ������������� ��� ����������� ������. ������� ������ �� �����.
    /// </summary>
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        PlayerName = ExternalListener.PlayerName;

        PlayerCamera.gameObject.SetActive(true);
    }

    /// <summary>
    /// ���������� ������������� ��� ��������� ������ �� �����. ����������� ������ ��� ����.
    /// </summary>
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
    }

    /// <summary>
    /// ���������� ������ ����. �������� �� �������� ������.
    /// </summary>
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

    /// <summary>
    /// ���������� ��� ������������ ������ � ������ ��������. 
    /// ���� ����� ������������ � ����, �� ������� �. 
    /// ���� � ������� ���������� � ���� ������ ��� � �������� ����� ������ � ����, � ���� ��� ������, � ��������� ��� ����, ��� ������.
    /// </summary>
    /// <param name="collision">��, � ��� ���������� �����</param>
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
                    f.SpawnFood();
                }
            }
        }
    }

    /// <summary>
    /// ����� ��� �������� ���������� ������� ������ � ��������� ������.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// ����� ����� ���� ��������� ���������� �� ������ � �������� ����� �� ����� ������ ��� ���.
    /// </summary>
    public void Death()
    {
        isAlive = false;

        PlayerRigidbody2D.velocity = Vector3.zero;
        CmdChangeCompState(false);

        if (hasAuthority)
            DeathCanvas.SetActive(true);

        ExternalListener.SendResults(this.GetComponent<Weight>().GetWeight().ToString());
    }

    /// <summary>
    /// ���������� ��� ������� ������ �������. ������ ������� ������ �� �����.
    /// </summary>
    public void Retry()
    {
        isAlive = true;

        CmdChangeCompState(true);
        
        DeathCanvas.SetActive(false);

        Weight.ResetWeight();
        this.gameObject.transform.localScale = Vector3.one;
    }
}
