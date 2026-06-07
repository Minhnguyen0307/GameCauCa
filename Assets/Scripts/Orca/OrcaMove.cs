using UnityEngine;

public class OrcaMove : MonoBehaviour
{
    public float speed = 1.2f;

    void Update()
    {
        if (FishFreezeManager.Instance != null && FishFreezeManager.Instance.IsFrozen)
            return;

        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}