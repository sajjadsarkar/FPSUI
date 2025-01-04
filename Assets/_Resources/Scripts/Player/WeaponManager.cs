using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour 
{
    private GameObject[] weaponsInUse = new GameObject[2];
    public GameObject[] weaponsInGame;
    public Rigidbody[] worldModels;

    public RaycastHit hit;
    private float dis = 3.0f;
    public LayerMask layerMaskWeapon;
    public LayerMask layerMaskAmmo;

    public Transform dropPosition;

    private float switchWeaponTime = 0.25f;
    public bool canSwitch = true;
	private bool equipped = false;
    [HideInInspector] public bool showWepGui = false;
    [HideInInspector] public bool showAmmoGui = false;
    [HideInInspector] public int weaponToSelect;
    [HideInInspector] public int setElement;
    [HideInInspector] public int weaponToDrop;
	[HideInInspector] public float crosshairSize;

    public AudioClip pickupSound;
    public AudioSource aSource;
    public HealthScript hs;
    private string textFromPickupScript = "";
    private string notes = "";
    private string note = "Press key <color=#88FF6AFF> << E >> </color> to pick up Ammo";
    private string wrongType = "Select appropriate weapon to pick up";
    public int selectWepSlot1 = 0;
    public int selectWepSlot2 = 0;
	Text noteUI;
	
    void Start()
    {
		noteUI = CanvasManager.instance.note;
		CanvasManager.instance.SetWeapon(this);
		
        for (int h = 0; h < worldModels.Length; h++)
        {
            weaponsInGame[h].SetActive(false);
        }

        weaponsInUse[0] = weaponsInGame[selectWepSlot1];
        weaponsInUse[1] = weaponsInGame[selectWepSlot2];

        weaponToSelect = 0;
        StartCoroutine(DeselectWeapon());
    }
	
    void Update() {
	
        if (Cursor.lockState == CursorLockMode.None) return;

        if (Input.GetKeyDown("1") && weaponsInUse.Length >= 1 && canSwitch && weaponToSelect != 0)
        {
            StartCoroutine(DeselectWeapon());
            weaponToSelect = 0;
        }
        else if (Input.GetKeyDown("2") && weaponsInUse.Length >= 2 && canSwitch && weaponToSelect != 1)
        {
            StartCoroutine(DeselectWeapon());
            weaponToSelect = 1;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && canSwitch)
        {
            weaponToSelect++;
            if (weaponToSelect > (weaponsInUse.Length - 1))
            {
                weaponToSelect = 0;
            }
            StartCoroutine(DeselectWeapon());
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && canSwitch)
        {
            weaponToSelect--;
            if (weaponToSelect < 0)
            {
                weaponToSelect = weaponsInUse.Length - 1;
            }
            StartCoroutine(DeselectWeapon());
        }

        Vector3 pos = transform.parent.position;
        Vector3 dir = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(pos, dir, out hit, dis, layerMaskWeapon))
        {
            WeaponIndex pre = hit.transform.GetComponent<WeaponIndex>();
            setElement = pre.setWeapon;
			
			if(!showWepGui)
			{			
				if (weaponsInUse[0] != weaponsInGame[setElement] && weaponsInUse[1] != weaponsInGame[setElement])
				{
					equipped = false;
					noteUI.text = "Press key <color=#88FF6AFF> << E >> </color> to pickup weapon";
				}
				else
				{
					equipped = true;
					noteUI.text = "Weapon is already equipped";
				}
				showWepGui = true;
			}
			
            if (canSwitch && !equipped)
            {
                if (Input.GetKeyDown("e"))
                {
                    DropWeapon(weaponToDrop);
                    StartCoroutine(DeselectWeapon());
                    weaponsInUse[weaponToSelect] = weaponsInGame[setElement];
                    Destroy(hit.transform.gameObject);
                }
            }
        }
        else
        {
			if(showWepGui)
			{	
				noteUI.text = "";
				showWepGui = false;
			}
        }

        if (Physics.Raycast(pos, dir, out hit, dis, layerMaskAmmo))
        {
            if (hit.transform.CompareTag("Ammo"))
            {
                Pickup pickupGO = hit.transform.GetComponent<Pickup>();

                //bullets/magazines
                if (pickupGO.pickupType == PickupType.Magazines)
                {
                    WeaponScriptNEW mags = weaponsInUse[weaponToSelect].transform.GetComponent<WeaponScriptNEW>();
                    if (mags == null)
                    {
                        textFromPickupScript = "";
                        return;
                    }
                    if (mags.firstMode != fireMode.launcher)
                    {
                        notes = "";
                        textFromPickupScript = note;
                        if (Input.GetKeyDown("e"))
                        {
                            if (mags.ammoMode == Ammo.Magazines)
                                mags.magazines += pickupGO.amount;
                            else
                                mags.magazines += pickupGO.amount * mags.bulletsPerMag;

							CanvasManager.instance.UpdateMags(mags.magazines);
							aSource.PlayOneShot(pickupSound, 0.3f);
                            Destroy(hit.transform.gameObject);
                        }
                    }
                    else
                    {
                        textFromPickupScript = pickupGO.AmmoInfo;
                        notes = wrongType;
                    }
                }

                //projectiles/rockets
                if (pickupGO.pickupType == PickupType.Projectiles)
                {
                    WeaponScriptNEW projectile = weaponsInUse[weaponToSelect].transform.GetComponent<WeaponScriptNEW>();
                    if (projectile == null)
                    {
                        textFromPickupScript = "";
                        return;
                    }
                    if (projectile.secondMode == fireMode.launcher || projectile.firstMode == fireMode.launcher)
                    {
                        notes = "";
                        textFromPickupScript = note;
                        if (Input.GetKeyDown("e"))
                        {
                            projectile.projectiles += pickupGO.amount;
							CanvasManager.instance.UpdateProjectileUI(projectile.projectiles); 
                            aSource.PlayOneShot(pickupSound, 0.3f);
                            Destroy(hit.transform.gameObject);
                        }
                    }
                    else
                    {
                        textFromPickupScript = pickupGO.AmmoInfo;
                        notes = wrongType;
                    }
                }

                //health
                if (pickupGO.pickupType == PickupType.Health)
                {
                    textFromPickupScript = pickupGO.AmmoInfo;
                    notes = "";
                    if (Input.GetKeyDown("e"))
                    {
                        hs.Medic(pickupGO.amount);
                        aSource.PlayOneShot(pickupSound, 0.3f);
                        Destroy(hit.transform.gameObject);
                    }
                }
            }
			
			if(!showAmmoGui)
			{
				noteUI.text = notes + "\n" + textFromPickupScript;
				showAmmoGui = true;
			}	
        }
        else
        {
			if(showAmmoGui)
			{	
				noteUI.text = "";
				showAmmoGui = false;
			}
        }
    }

    IEnumerator DeselectWeapon()
    {
        canSwitch = false;

        for (int i = 0; i < weaponsInUse.Length; i++)
        {
			if(weaponsInUse[i].GetComponent<WeaponScriptNEW>() != null)
				weaponsInUse[i].GetComponent<WeaponScriptNEW>().Deselect();
            weaponsInUse[i].gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(switchWeaponTime);
        SelectWeapon(weaponToSelect);
        yield return new WaitForSeconds(switchWeaponTime);
        canSwitch = true;
    }
	
    void SelectWeapon(int i)
    {
        weaponsInUse[i].gameObject.SetActive(true);
        weaponsInUse[i].SendMessage("DrawWeapon", SendMessageOptions.DontRequireReceiver);
        WeaponIndex temp = weaponsInUse[i].transform.GetComponent<WeaponIndex>();
        weaponToDrop = temp.setWeapon;
    }

    void DropWeapon(int index)
    {
        if (index == 0) return;

        for (int i = 0; i < worldModels.Length; i++)
        {
            if (i == index)
            {
                Rigidbody drop = Instantiate(worldModels[i], dropPosition.transform.position, dropPosition.transform.rotation) as Rigidbody;
                drop.AddRelativeForce(0, 250, Random.Range(100, 200));
                drop.AddTorque(-transform.up * 40);
            }
        }
    }

    public void EnterWater()
    {
        canSwitch = false;
        for (int i = 0; i < weaponsInUse.Length; i++)
        {
			if(weaponsInUse[i].GetComponent<WeaponScriptNEW>() != null)
				weaponsInUse[i].GetComponent<WeaponScriptNEW>().Deselect();
            weaponsInUse[i].gameObject.SetActive(false);
        }
    }

    public void ExitWater()
    {
        canSwitch = true;
        SelectWeapon(weaponToSelect);
    }
}