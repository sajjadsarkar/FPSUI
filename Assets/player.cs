using UnityEngine;

public class Player : MonoBehaviour
{
    private bool isRotating = false;
    private float rotationSpeed = 5f;

    void OnMouseDown()
    {
        if (Input.GetMouseButton(0)) // Left mouse button
        {
            isRotating = true;
        }
    }

    void OnMouseUp()
    {
        isRotating = false;
    }

    void Update()
    {
        if (isRotating)
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up, -mouseX * rotationSpeed);
        }
    }
}
