using UnityEngine;

public class OrcaMove : MonoBehaviour
{
    public float speed = 1.2f;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        //if (FishFreezeManager.Instance.IsFrozen)
        //    return;

        //transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}