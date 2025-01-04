using UnityEngine;
using System.Collections;

public class Medic : MonoBehaviour
{
    public float hitPoints = 50.0f;
    public AudioClip sound;
	public AudioSource aSource;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessageUpwards("Medic", hitPoints, SendMessageOptions.DontRequireReceiver);
            AudioSource.PlayClipAtPoint(sound, transform.position, 0.3f);		
            Destroy(gameObject);
        }
    }
}