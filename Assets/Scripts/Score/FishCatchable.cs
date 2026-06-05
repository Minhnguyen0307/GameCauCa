using UnityEngine;
using System.Collections;

public class FishCatchable : MonoBehaviour
{
    public int scoreValue = 10;

    [Range(0f, 1f)]
    public float catchChance = 0.5f;

    public float shakeDuration = 0.15f;
    public float shakeStrength = 0.1f;

    private bool isCaught = false;
    private Coroutine shakeCoroutine;

    // =========================
    // BẮT CÁ BÌNH THƯỜNG
    // =========================
    public void TryCatch()
    {
        if (isCaught) return;

        if (Random.value <= catchChance)
        {
            Catch();
        }
        else
        {
            PlayShake();
        }
    }

    // =========================
    // BẮT 100% (ITEM MU)
    // =========================
    public void ForceCatch()
    {
        if (isCaught) return;
        Catch();
    }

    void Catch()
    {
        isCaught = true;

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddScore(scoreValue);

        Destroy(gameObject);
    }

    // =========================
    void PlayShake()
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        Vector3 startPos = transform.position; // ✅ LẤY VỊ TRÍ HIỆN TẠI

        float timer = 0f;
        while (timer < shakeDuration)
        {
            timer += Time.deltaTime;

            float x = Random.Range(-shakeStrength, shakeStrength);
            float y = Random.Range(-shakeStrength, shakeStrength);

            transform.position = startPos + new Vector3(x, y, 0f);
            yield return null;
        }

        transform.position = startPos; // ✅ TRẢ VỀ ĐÚNG CHỖ ĐANG ĐỨNG
    }
}