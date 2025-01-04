using UnityEngine;

public class Activate : MonoBehaviour, IDamagable
{
    public GameObject GO;

	public void ApplyDamage(int damage)
    {
        Action();
    }
	
	public void ApplyExplosionDamage (int damage)
	{
		Action();
	}

    void Action()
    {
        GO.SendMessage("Action", SendMessageOptions.DontRequireReceiver);
    }
}