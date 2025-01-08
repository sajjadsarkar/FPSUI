using UnityEngine;

public class Player : MonoBehaviour
{
    private bool isRotating = false;
    private float rotationSpeed = 5f;
    private float previousTouchX;

    void OnMouseDown()
    {
        isRotating = true;
        if (Input.touchCount > 0)
        {
            previousTouchX = Input.GetTouch(0).position.x;
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
            if (Input.touchCount > 0)
            {
                // Handle touch input
                Touch touch = Input.GetTouch(0);
                float currentTouchX = touch.position.x;
                float deltaX = currentTouchX - previousTouchX;
                transform.Rotate(Vector3.up, -deltaX * rotationSpeed * Time.deltaTime);
                previousTouchX = currentTouchX;
            }
            else
            {
                // Handle mouse input
                float mouseX = Input.GetAxis("Mouse X");
                transform.Rotate(Vector3.up, -mouseX * rotationSpeed);
            }
        }
    }
}