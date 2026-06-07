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

        transform.Translate(Vector3.left * speed * Time.deltaTime);

        float cameraLeft =
            cam.transform.position.x - cam.orthographicSize * cam.aspect;

        if (transform.position.x < cameraLeft - destroyOffset)
        {
            Destroy(gameObject);
        }
    }
}