using UnityEngine;

public class OrcaMove : MonoBehaviour
{
    public float speed = 1.2f;
    public float destroyOffset = 5f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (FishFreezeManager.Instance != null && FishFreezeManager.Instance.IsFrozen)
            return;

        float speedFactor = (FishDoubleManager.Instance != null && FishDoubleManager.Instance.IsSlowDownActive) ? 0.5f : 1f;
        transform.Translate(Vector2.right * speed * speedFactor * Time.deltaTime);

        if (cam == null) cam = Camera.main;
        if (cam != null)
        {
            float cameraRight = cam.transform.position.x + cam.orthographicSize * cam.aspect;
            if (transform.position.x > cameraRight + destroyOffset)
            {
                Destroy(gameObject);
            }
        }
    }
}