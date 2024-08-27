using UnityEngine;

public class AlignWithCamera : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();
    }

    private void Update()
    {
        if (mainCamera != null)
        {
            transform.rotation = Quaternion.Euler(mainCamera.transform.eulerAngles.x, mainCamera.transform.eulerAngles.y, 0);
        }
    }
}