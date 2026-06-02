using UnityEngine;

public class BombFall : MonoBehaviour
{
    public float fallSpeed = 5f;
    private float bottomY;
    private bool stopped = false;

    void Start()
    {
        bottomY = Camera.main.ViewportToWorldPoint(
            new Vector3(0, 0, 0)
        ).y + 0.5f;
    }

    void Update()
    {
        if (stopped) return;

        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

        if (transform.position.y <= bottomY)
        {
            transform.position = new Vector2(
                transform.position.x,
                bottomY
            );
            stopped = true;
        }
    }
}