using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Weight))]
public class PlayerEating : MonoBehaviour
{
    [SerializeField] private Weight _weight;
    private float cameraSizeOffset;

    private void Start()
    {
        cameraSizeOffset = this.GetComponent<PlayerMovement>().PlayerCamera.orthographicSize;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Weight>(out Weight _w))
        {
            if (_weight.GetWeight() > _w.GetWeight())
            {
                _weight.AddWeight(_w.GetWeight());
                StopAllCoroutines();
                StartCoroutine(ChangeScale());
                Destroy(collision.gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        //float weightDecay = -Time.deltaTime * _weight.GetWeight() / 100; 
        //_weight.AddWeight(weightDecay);
        //StartCoroutine(ChangeScale());
    }

    private IEnumerator ChangeScale()
    {
        float currentScale = this.transform.localScale.x;
        float currentCameraSize = this.GetComponent<PlayerMovement>().PlayerCamera.orthographicSize;
        float scaleInterpolation;
        float cameraScaleInterpolation;

        for (float t = 0; t <= 1f; t += Time.deltaTime)
        {
            scaleInterpolation = Mathf.Lerp(currentScale, _weight.GetWeight(), t);
            this.transform.localScale = new Vector3(scaleInterpolation, scaleInterpolation, 1);

            cameraScaleInterpolation = Mathf.Lerp(currentCameraSize, _weight.GetWeight() + cameraSizeOffset - 1f, t);
            this.GetComponent<PlayerMovement>().PlayerCamera.orthographicSize = cameraScaleInterpolation;

            yield return null;
        }
    }
}
