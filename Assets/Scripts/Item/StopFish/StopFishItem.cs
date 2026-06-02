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
        Time.timeScale = 0f;      // Dừng toàn game

        yield return new WaitForSecondsRealtime(freezeTime);

        Time.timeScale = 1f;      // Chạy lại

        Destroy(gameObject);      // Xóa item
    }
}