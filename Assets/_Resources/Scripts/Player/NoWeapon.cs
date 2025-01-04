using UnityEngine;

public class NoWeapon : MonoBehaviour
{
    void DrawWeapon()
    {
		CanvasManager.instance.UpdateAmmoUI(0, 0, 0);
		CanvasManager.instance.ShowProjectilesUI(false);
		CanvasManager.instance.ShowCrosshair(0);
    }

    void Deselect()
    {
    }
}