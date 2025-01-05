using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;
    private FPSController fpsController;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        fpsController = FindObjectOfType<FPSController>();
    }

    public void OnJumpButtonPressed()
    {
        if (fpsController != null)
            fpsController.DoJump();
    }

    public void OnRunButtonPressed()
    {
        if (fpsController != null)
            fpsController.ToggleRun();
    }

    public void OnCrouchButtonPressed()
    {
        if (fpsController != null)
            fpsController.ToggleCrouch();
    }

    public void OnProneButtonPressed()
    {
        if (fpsController != null)
            fpsController.ToggleProne();
    }
    public void OnPickupButtonPressed()
    {
        WeaponManager weaponManager = FindObjectOfType<WeaponManager>();
        if (weaponManager != null)
            weaponManager.PickupWeapon();
    }
}