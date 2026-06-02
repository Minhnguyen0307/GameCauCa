using UnityEngine;
using TMPro;

public class SharkWarning : MonoBehaviour
{
    public TextMeshProUGUI warningText;
    public float warningDistance = 5f;

    Camera cam;
    GameObject shark;

    void Start()
    {
        cam = Camera.main;
        warningText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (shark == null)
        {
            shark = GameObject.FindGameObjectWithTag("Shark");
            if (shark == null)
            {
                warningText.gameObject.SetActive(false);
                return;
            }
        }

        // Mép PHẢI camera
        float cameraRight =
            cam.transform.position.x + cam.orthographicSize * cam.aspect;

        float sharkX = shark.transform.position.x;

        // Khoảng cách từ cá mập tới mép phải camera
        float distanceToCamera = sharkX - cameraRight;

        // DEBUG – bạn nên bật để kiểm tra
        Debug.Log("Distance to camera: " + distanceToCamera);

        // 👉 CHỈ CẢNH BÁO KHI CHƯA VÀO CAMERA
        if (distanceToCamera > 0 && distanceToCamera <= warningDistance)
        {
            warningText.gameObject.SetActive(true);
        }
        else
        {
            warningText.gameObject.SetActive(false);
        }
    }
}