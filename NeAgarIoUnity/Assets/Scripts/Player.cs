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

    [Header("Settings")]
    [SerializeField] private float Speed = 1;

    private Vector2 AllowedRadius = GameGlobalSettings.GameField;
    private float cameraSizeOffset;

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
    }

    private void FixedUpdate()
    {
        if (!hasAuthority) { return; }

        //disabled for testing
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Weight>(out Weight _w))
        {
            if (Weight.GetWeight() > _w.GetWeight())
            {
                Weight.AddWeight(_w.GetWeight() / 10);
                StopAllCoroutines();
                StartCoroutine(ChangeScale());
                Destroy(collision.gameObject);

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
}
