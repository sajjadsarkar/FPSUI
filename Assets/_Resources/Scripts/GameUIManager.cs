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
    public void OnReloadButtonPressed()
    {
        WeaponScriptNEW weapon = FindObjectOfType<WeaponScriptNEW>();
        if (weapon != null && weapon.selected)
        {
            StartCoroutine(weapon.Reload());
        }
    }
    public void OnPickupButtonPressed()
    {
        WeaponManager weaponManager = FindObjectOfType<WeaponManager>();
        if (weaponManager != null)
            weaponManager.PickupWeapon();
    }
    public void OnFireButtonPressed()
    {
        WeaponScriptNEW weapon = FindObjectOfType<WeaponScriptNEW>();
        if (weapon != null && weapon.selected)
        {
            if (weapon.currentMode == fireMode.semi || weapon.currentMode == fireMode.auto)
            {
                weapon.FireSemi();
            }
            else if (weapon.currentMode == fireMode.launcher)
            {
                weapon.FireLauncher();
            }
            else if (weapon.currentMode == fireMode.burst)
            {
                StartCoroutine(weapon.FireBurst());
            }
            else if (weapon.currentMode == fireMode.shotgun)
            {
                weapon.FireShotgun();
            }
        }
    }

}