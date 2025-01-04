using UnityEngine;
using System.Collections;

public class ObjectDamage : MonoBehaviour, IDamagable
{
    public Target mainDamageReceiver;
    public float multiplier;
    public bool head = false;

	public void ApplyDamage(int damage)
    {
        mainDamageReceiver.FinalDamage(damage * multiplier, head);
    }
	
	public void ApplyExplosionDamage (int damage)
	{
		mainDamageReceiver.FinalDamage(damage * multiplier, head);
	}
}