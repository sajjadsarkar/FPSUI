using UnityEngine;
using System.Collections;

public class DamageonTriggerEnter : MonoBehaviour
{
    public float vel = 1.0f;
    public AudioSource aSource;
    public Rigidbody rb;

    void OnTriggerEnter(Collider other)
    {
        vel = rb.linearVelocity.magnitude;
        if (vel > 1)
        {
            aSource.volume = vel / 40;
           if(aSource.enabled) aSource.Play();
            if (other.CompareTag("Enemy") || other.CompareTag("Metal"))
            {
                other.BroadcastMessage("ApplyDamage", vel * 3000, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}