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
		if (Cursor.lockState == CursorLockMode.None)
		{
			return;
		}
		float inputX = 0;
		float inputY = 0;
#if UNITY_ANDROID || UNITY_IOS
		// Use the entire screen for look/rotation
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			// Enhanced UI check - using IsPointerOverGameObject
			bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId) ||
						   UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

			if (!isOverUI)
			{
				inputX = touch.deltaPosition.x * sensitivityX * 0.1f;
				inputY = touch.deltaPosition.y * sensitivityY * 0.1f;
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