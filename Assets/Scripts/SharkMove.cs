using UnityEngine;

public class SharkMove : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        if (FishFreezeManager.Instance != null && FishFreezeManager.Instance.IsFrozen)
            return;

        float speedFactor = (FishDoubleManager.Instance != null && FishDoubleManager.Instance.IsSlowDownActive) ? 0.5f : 1f;
        transform.Translate(Vector3.left * speed * speedFactor * Time.deltaTime);
    }
}