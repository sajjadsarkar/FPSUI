using UnityEngine;

public class Destroyer : MonoBehaviour
{
	public ParticleSystem particles;

    public void DestroyNow()
    {
		particles.Stop();
        Destroy(gameObject, 5.0f);
    }
}