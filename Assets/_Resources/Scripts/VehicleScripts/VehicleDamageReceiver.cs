using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleDamageReceiver : MonoBehaviour, IDamagable {

	public VehicleDamage vehicleDamage;
	
	public void ApplyDamage(int damage)
    {
		int damageFromWeapon = (int)(damage/5);
		vehicleDamage.ApplyDamage(damageFromWeapon);
    }
	
	public void ApplyExplosionDamage (int damage)
	{
		vehicleDamage.ApplyDamage(damage);
	}
}
