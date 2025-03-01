using UnityEngine;

[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour
{

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -60F;
	public float maximumX = 60F;
	public float minimumY = -60F;
	public float maximumY = 60F;

	public float offsetY = 0F;
	public float offsetX = 0F;

	public float rotationX = 0F;
	GameObject cmra = null;
	public float rotationY = 0F;

	private Quaternion originalRotation;
	private Vector2 touchDelta;
	private bool isTouching = false;

	void Update()
	{
		float inputX = 0;
		float inputY = 0;

#if UNITY_ANDROID || UNITY_IOS
		// Check for second touch for rotation
		if (Input.touchCount > 1)
		{
			Touch touch = Input.GetTouch(1); // Use second finger for look/rotation

			if (touch.phase == TouchPhase.Moved)
			{
				// Only rotate if not over UI elements
				if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId))
				{
					inputX = touch.deltaPosition.x * sensitivityX * 0.1f;
					inputY = touch.deltaPosition.y * sensitivityY * 0.1f;
				}
			}
		}
		// Fallback to first touch if only one finger is used
		else if (Input.touchCount == 1)
		{
			Touch touch = Input.GetTouch(0);

			// Check right side of screen for rotation
			if (touch.position.x > Screen.width * 0.5f)
			{
				if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId))
				{
					inputX = touch.deltaPosition.x * sensitivityX * 0.1f;
					inputY = touch.deltaPosition.y * sensitivityY * 0.1f;
				}
			}
		}
#endif

		if (axes == RotationAxes.MouseXAndY)
		{
			rotationX += inputX;
			rotationY += inputY;

			rotationX = ClampAngle(rotationX, minimumX, maximumX);
			rotationY = ClampAngle(rotationY, minimumY, maximumY);

			Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
			Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);

			transform.localRotation = originalRotation * xQuaternion * yQuaternion;
		}
		else if (axes == RotationAxes.MouseX)
		{
			rotationX += (inputX * sensitivityX / 60 * cmra.GetComponent<Camera>().fieldOfView + offsetX);
			rotationX = ClampAngle(rotationX, minimumX, maximumX);

			Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
			transform.localRotation = originalRotation * xQuaternion;
		}
		else
		{
			rotationY += (inputY * sensitivityY / 60 * cmra.GetComponent<Camera>().fieldOfView + offsetY);
			rotationY = ClampAngle(rotationY, minimumY, maximumY);

			Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);
			transform.localRotation = originalRotation * yQuaternion;
		}

		offsetY = 0F;
		offsetX = 0F;
	}

	void Start()
	{
		cmra = GameObject.FindWithTag("MainCamera");
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
		originalRotation = transform.localRotation;
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}

	public void SetRotation(float r)
	{
		rotationX = r;
	}
}