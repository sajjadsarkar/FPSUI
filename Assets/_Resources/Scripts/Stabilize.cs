using UnityEngine;

public class Stabilize : MonoBehaviour 
{
    public float speed = 10.0f;
	public Rigidbody rigid;
	public Wheel[] wheels;

    void FixedUpdate () 
	{
		int multiplier = 0;
		for(int i = 0; i < wheels.Length; i++)
		{
			if(wheels[i].onGround == false) multiplier ++;
		}	
        Vector3 up = Quaternion.AngleAxis(rigid.angularVelocity.magnitude * Mathf.Rad2Deg * speed, rigid.angularVelocity) * transform.up;
        Vector3 torqueVector = Vector3.Cross(up, Vector3.up);
        rigid.AddTorque(torqueVector * speed * multiplier);	
    }
}
