using UnityEngine;
using System.Collections;

public class PoolTrigger : MonoBehaviour 
{
	void OnTriggerEnter (Collider other)
	{
		
		if(other.CompareTag("Player"))
			other.GetComponent<FPSController>().PlayerInWater(transform.position.y);	
		else
		{
			Buoyancy b = other.GetComponent<Buoyancy>();
			if(b) b.enabled = true;	
			
			if(other.gameObject.name == "VehicleEnter")
			{
				VehicleScript v = other.GetComponent<VehicleScript>();
				if(v)
				{
					v.GetOut();
					v.UnderWater();
				}	
			}
		}
	}
}