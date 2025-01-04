using UnityEngine;
using System.Collections;

public class LandMineDamage : MonoBehaviour, IDamagable
{
    public GameObject explosion;
    private bool activated = false;

    void OnTriggerEnter(Collider other)
    {
        Explosion();
    }

	public void ApplyDamage(int damage)
    {
        Explosion();
    }
	
	public void ApplyExplosionDamage (int damage)
	{
		Explosion();
	}

    void Explosion()
    {
        if (activated) return;
        activated = true;

        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}