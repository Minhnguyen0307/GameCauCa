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

        // Cá mập ăn được tất cả cá (trừ Orca)
        if (other.CompareTag("Shark"))
        {
            Destroy(gameObject);
        }
    }

    // Hỗ trợ cả trường hợp Shark dùng Collider vật lý
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isVisible) return;

        if (collision.collider.CompareTag("Shark"))
        {
            Destroy(gameObject);
        }
    }
}