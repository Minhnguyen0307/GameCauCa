using UnityEngine;

public class CrabMove : MonoBehaviour
{
    public float speed = 2f;
    public float groundY = -4.5f; // ← đáy màn hình

    private int direction = 1;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        // Ép cua luôn ở đáy ngay từ đầu
        transform.position = new Vector3(
            transform.position.x,
            groundY,
            transform.position.z
        );
    }

    void Update()
    {
        // Di chuyển ngang
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // Luôn khóa Y
        transform.position = new Vector3(
            transform.position.x,
            groundY,
            transform.position.z
        );

        // Biên màn hình
        float leftEdge = cam.transform.position.x - cam.orthographicSize * cam.aspect;
        float rightEdge = cam.transform.position.x + cam.orthographicSize * cam.aspect;

        if (transform.position.x < leftEdge || transform.position.x > rightEdge)
        {
            direction *= -1;
            Flip();
        }

        //if (FishFreezeManager.Instance.IsFrozen)
        //    return;

        //transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}