using UnityEngine;
using System.Collections;

public class StopFishItem : MonoBehaviour
{
    public float freezeTime = 5f;

    private bool used = false;

    void OnMouseDown()
    {
        if (used) return;
        used = true;

        StartCoroutine(FreezeFish());
    }

    IEnumerator FreezeFish()
    {
        // Freeze cá qua FishFreezeManager, không đụng timeScale
        FishFreezeManager.Instance.FreezeAllFish(freezeTime);

        yield return new WaitForSecondsRealtime(freezeTime);

        Destroy(gameObject);      // Xóa item
    }
}