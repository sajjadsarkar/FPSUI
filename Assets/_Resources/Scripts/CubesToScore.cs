using UnityEngine;

public class CubesToScore : MonoBehaviour {
	
	public AudioSource aSource;
	public AudioClip sound;

	void OnTriggerEnter (Collider other) 
	{
		if(other.GetComponent<Rigidbody>())
		{
			Reward reward = other.GetComponent<Reward>();
			if(reward)
			{
				ScoreManager.instance.AddScore(reward.score);
				aSource.PlayOneShot(sound);
				Destroy(other);
			}
		}		
	}
}
