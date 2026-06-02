using UnityEngine;

public class FishEatable : MonoBehaviour
{
    private bool isVisible = false;

    void OnBecameVisible()
    {
        isVisible = true;
    }

    void OnBecameInvisible()
    {
        isVisible = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isVisible) return;

        if (other.CompareTag("Shark"))
        {
            Destroy(gameObject);
        }
    }
}