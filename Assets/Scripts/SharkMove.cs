using UnityEngine;

public class SharkMove : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        if (FishFreezeManager.Instance != null && FishFreezeManager.Instance.IsFrozen)
            return;

        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}