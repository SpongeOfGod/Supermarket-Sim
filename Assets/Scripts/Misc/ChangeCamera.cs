using UnityEngine;
using UnityEngine.UI;

public class CameraSwitch : MonoBehaviour
{
    public Transform[] cameraPositions;

    private Camera mainCamera;
    private int currentIndex = 0;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void OnButtonClick()
    {
        Transform target = cameraPositions[currentIndex];
        mainCamera.transform.position = target.position;
        mainCamera.transform.rotation = target.rotation;

        currentIndex = (currentIndex + 1) % cameraPositions.Length;
    }
}
