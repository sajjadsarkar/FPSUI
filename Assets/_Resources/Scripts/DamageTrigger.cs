using UnityEngine;

public class DamageTrigger : MonoBehaviour {
	
	public Rigidbody rigid;
	public VehicleDamage vDamage;
	public AudioSource aSource;
	
	void OnTriggerEnter (Collider other) {
		if(other.CompareTag("Player") || other.GetComponent<Rigidbody>()) return;
		float speed = rigid.linearVelocity.magnitude;

		if(speed > 7.0f){
			vDamage.ApplyDamage((int)speed);
			aSource.volume = 0.2f + (speed/30);
			aSource.Play();	
		}	
	}
}