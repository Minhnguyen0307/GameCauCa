using UnityEngine;

public class SharkMove : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        //if (FishFreezeManager.Instance.IsFrozen)
        //    return;

        //transform.Translate(Vector3.left * speed * Time.deltaTime);
    }


}