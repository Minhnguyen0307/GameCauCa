using UnityEngine;

public class SpecialFishMove : MonoBehaviour
{
    public float speed = 12f;
    public float destroyOffset = 2f;

    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (FishFreezeManager.Instance != null && FishFreezeManager.Instance.IsFrozen)
            return;

        float speedFactor = (FishDoubleManager.Instance != null && FishDoubleManager.Instance.IsSlowDownActive) ? 0.5f : 1f;
        transform.Translate(Vector3.left * speed * speedFactor * Time.deltaTime);

        float cameraLeft =
            cam.transform.position.x - cam.orthographicSize * cam.aspect;

        if (transform.position.x < cameraLeft - destroyOffset)
        {
            Destroy(gameObject);
        }
    }
}