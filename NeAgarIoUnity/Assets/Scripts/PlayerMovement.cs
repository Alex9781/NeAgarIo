using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    public Camera PlayerCamera;

    [SerializeField] private float Speed = 1;

    private Vector2 AllowedRadius = GameGlobalSettings.GameField;
    private Rigidbody2D _rb;

    private void Start()
    {
        PlayerCamera = this.gameObject.GetComponentInChildren<Camera>();
        this.gameObject.TryGetComponent<Rigidbody2D>(out _rb);
    }

    private void FixedUpdate()
    {
        Vector2 cursorPos = PlayerCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 vectorDelta = cursorPos - new Vector2(
            this.gameObject.transform.position.x,
            this.gameObject.transform.position.y);

        _rb.velocity = new Vector2(
            Mathf.Clamp(vectorDelta.x, -Speed, Speed),
            Mathf.Clamp(vectorDelta.y, -Speed, Speed)).normalized * Speed;

        this.transform.position = new Vector3(
            Mathf.Clamp(this.transform.position.x, -AllowedRadius.x, AllowedRadius.x),
            Mathf.Clamp(this.transform.position.y, -AllowedRadius.y, AllowedRadius.y),
            0);
    }
}
