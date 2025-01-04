using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    float explosionRadius = 5.0f;
    float explosionPower = 10.0f;
    float explosionDamage = 100.0f;
    float explosionTimeout = 2.0f;

    void Start()
    {
        Vector3 explosionPosition = transform.position;

        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
        foreach (var hit in colliders)
        {
			IDamagable damagable = hit.GetComponent<Collider>().GetComponent<IDamagable>();
			if(damagable != null)
			{
				Vector3 closestPoint = hit.ClosestPointOnBounds(explosionPosition);
				float distance = Vector3.Distance(closestPoint, explosionPosition);

				float hitPoints = 1.0f - Mathf.Clamp01(distance / explosionRadius);
				hitPoints *= explosionDamage;
				
				damagable.ApplyExplosionDamage((int)hitPoints);
			}
        }

        colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Rigidbody>())
                hit.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, explosionPosition, explosionRadius, 3.0f);
        }

        Destroy(gameObject, explosionTimeout);
    }
}