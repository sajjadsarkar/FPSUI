using UnityEngine;
using System.Collections;

public class RotateGrenade : MonoBehaviour 
{
	public Transform explosionPrefab;
	float rotationSpeed = 100.0f; 

	void Update ()
	{
		transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);	
	}

	void OnCollisionEnter (Collision collision)
	{
		// Rotate the object so that the y-axis faces along the normal of the surface
		ContactPoint contact = collision.contacts[0];
		Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
		Vector3 pos = contact.point;
		Instantiate(explosionPrefab, pos, rot);
		// Destroy the projectile
		Destroy (gameObject);
	}
}