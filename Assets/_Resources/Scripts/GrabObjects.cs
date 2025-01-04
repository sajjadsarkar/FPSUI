using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObjects : MonoBehaviour 
{
	public float grabPower = 10.0f; 
	public float throwPower = 25.0f;
	public float RayDistance = 3.0f;
	public LayerMask layerMask;
	private bool grab, drop = false;

	public Transform pos;
	public float adjust;
	Rigidbody obj;
	
	void Update ()
	{ 
	
		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			RaycastHit hit;
			if(Physics.Raycast(transform.position, transform.forward, out hit, RayDistance, layerMask.value))
			{
				Pickable pickable = hit.collider.GetComponent<Pickable>();
				if(pickable)
				{
					grab = true;
					obj = hit.rigidbody;
					obj.isKinematic = true;
					obj.GetComponent<Collider>().enabled = false;
					obj.transform.parent = pos;
				}		
			}
		}	
		
		if (Input.GetKeyUp(KeyCode.Mouse1))
		{ 
			if(grab)
			{
				StartCoroutine(PrepareToDrop(3f));
			}	
		}
		
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{ 
			if(grab)
			{
				StartCoroutine(PrepareToDrop(throwPower));
			}	
		}

		if(grab)
		{	
			obj.transform.position = Vector3.Lerp(obj.transform.position, pos.position + (pos.transform.forward * adjust) - (pos.transform.up * 0.4f), Time.deltaTime * grabPower); 
			obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, pos.rotation, Time.deltaTime * 5f);
		}
	} 

	IEnumerator PrepareToDrop(float power)
	{
		RaycastHit hits;
		while(!drop)
		{
			if(Physics.Raycast(transform.position, transform.forward, out hits, 1.5f, layerMask.value)) 
				drop = false;
			else 
				drop = true;
			yield return null;
		}
		
		obj.transform.parent = null;
		obj.isKinematic = false;
		obj.GetComponent<Collider>().enabled = true;
		obj.linearVelocity = transform.forward * power;
		drop = grab = false;
	}
}
