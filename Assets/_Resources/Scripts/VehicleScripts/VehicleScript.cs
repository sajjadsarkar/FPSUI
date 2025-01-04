using UnityEngine;
using System.Collections;

public class VehicleScript : MonoBehaviour
{
    private GameObject weaponCamera;
    public Transform vehicleCameraTarget;
	public VehicleCamera vehicleCamera;
	public VehicleDamage vehicleDamage;
    public GameObject vehicle;
    private GameObject player;
    public Transform GetOutPosition;
    private float waitTime = 0.5f;

    private GameObject mainCamera;
    [HideInInspector] public bool inVehicle = false;
	FPSController controller;
	
    void Start()
    {
		vehicleCamera.InVehicle(false, null);
		vehicle.GetComponent<CarController>().Status(false);
    }
	
    void Update()
    {
        if (!inVehicle) return;
        if (Input.GetKeyDown("e")) GetOut();
    }

    void Action()
    {
        if (!inVehicle) StartCoroutine(GetIn());
    }

    IEnumerator GetIn()
    {
		CanvasManager.instance.PlayerInVehicle(true);
        if(player == null) player = GameObject.FindWithTag("Player");
        if(mainCamera == null) mainCamera = GameObject.FindWithTag("MainCamera");
        if(weaponCamera == null) weaponCamera = GameObject.FindWithTag("WeaponCamera");
		if(controller == null) controller = player.GetComponent<FPSController>();
		
		controller.wm.EnterWater();
        player.SetActive(false);

		vehicleCamera.InVehicle(true, vehicleCameraTarget);
        player.transform.parent = vehicle.transform;
        player.transform.position = vehicleCameraTarget.transform.position;

        weaponCamera.GetComponent<Camera>().enabled = false;
        mainCamera.GetComponent<AudioListener>().enabled = false;
        mainCamera.GetComponent<Camera>().enabled = false;
        vehicle.GetComponent<CarController>().Status(true);
		vehicleDamage.UpdateHealthUI();
        yield return new WaitForSeconds(waitTime);
        inVehicle = true;
    }

    public void GetOut()
    {
		if(inVehicle)
		{
			player.transform.parent = null;
			player.transform.position = GetOutPosition.position;
			player.SetActive(true);
			player.GetComponent<MouseLook>().SetRotation(GetOutPosition.transform.rotation.eulerAngles.y);
			controller.wm.ExitWater();

			weaponCamera.GetComponent<Camera>().enabled = true;
			mainCamera.GetComponent<AudioListener>().enabled = true;
			mainCamera.GetComponent<Camera>().enabled = true;
			vehicleCamera.InVehicle(false, null);
			vehicle.GetComponent<CarController>().Status(false);
		}
        inVehicle = false;
		CanvasManager.instance.PlayerInVehicle(inVehicle);
    }
	
	public void UnderWater()
	{
		gameObject.SetActive(false);
		vehicle.GetComponent<Rigidbody>().linearDamping = 10;
	}	
}