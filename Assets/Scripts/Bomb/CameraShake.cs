using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private Vector3 originalPos;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        originalPos = transform.localPosition;
    }

    public void Shake(float duration = 0.15f, float strength = 0.08f)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine(duration, strength));
    }

    IEnumerator ShakeCoroutine(float duration, float strength)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;

            Vector2 offset = Random.insideUnitCircle * strength;
            transform.localPosition = originalPos + new Vector3(offset.x, offset.y, 0);

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}