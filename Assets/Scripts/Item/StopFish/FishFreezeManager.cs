using UnityEngine;
using System.Collections;

public class FishFreezeManager : MonoBehaviour
{
    public static FishFreezeManager Instance;

    private bool isFreezing = false;

    void Awake()
    {
        Instance = this;
    }

    public void FreezeAllFish(float duration)
    {
        if (isFreezing) return;

        StartCoroutine(FreezeCoroutine(duration));
    }

    IEnumerator FreezeCoroutine(float duration)
    {
        isFreezing = true;

        // 🔹 RUNG CAMERA NGAY LẬP TỨC
        CameraShake.Instance.Shake();

        // 🔹 DỪNG THỜI GIAN
        Time.timeScale = 0f;

        // 🔹 BỘ ĐẾM NGƯỢC 5s
        float timer = duration;
        while (timer > 0)
        {
            Debug.Log("Fish Freeze: " + Mathf.Ceil(timer) + "s");
            yield return new WaitForSecondsRealtime(1f);
            timer--;
        }

        // 🔹 HẾT ĐÓNG BĂNG
        Time.timeScale = 1f;
        isFreezing = false;
    }
}