using UnityEngine;

public class OrcaEat : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Shark"))
        {
            Destroy(other.gameObject);
        }
    }
}