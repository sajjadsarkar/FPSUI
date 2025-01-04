using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour, IDamagable
{
    public float hitPoints;
    public int maxHitPoints;
    public bool regeneration = false;
    public float regenerationSpeed;
    public AudioSource aSource;
    public AudioClip painSound;
    public AudioClip fallDamageSound;
    public Transform deadReplacement;

    private float t = 0.0f;
    private float alpha;
    private bool isDead = false;
    private ScoreManager scoreManager;
    public Transform camShake;
	private Vector3 originalPos;
	
	Image damageImage;
	Text healthText;

    void Start()
    {
		healthText = CanvasManager.instance.healthText;
		damageImage = CanvasManager.instance.damageImage;
		scoreManager = ScoreManager.instance;
		originalPos = camShake.localPosition;
		
        if (regeneration)
            hitPoints = maxHitPoints;
        alpha = 0.0f;
    }

    void Update()
    {
        if (t > 0.0f)
        {
            t -= Time.deltaTime;
            alpha = t;
			
			if(hitPoints > 0) 
			{
				Color col = damageImage.color;
				col.a = alpha;
				damageImage.color = col;
			}	
        }

        if (regeneration)
        {
            if (hitPoints < maxHitPoints)
                hitPoints += Time.deltaTime * regenerationSpeed;
        }	
    }

    public void ApplyDamage(int damage)
    {
        if (hitPoints < 0.0f) return;

        hitPoints -= damage;
		healthText.text = "+ " + hitPoints.ToString();
        aSource.PlayOneShot(painSound, 1.0f);
        t = 2.0f;

        if (hitPoints <= 0.0f) Die();
    }
	
	public void ApplyExplosionDamage (int damage)
	{
		if (hitPoints < 0.0f) return;
		StartCoroutine(Shake(damage));
		
        hitPoints -= damage;
		healthText.text = "+ " + hitPoints.ToString();
        aSource.PlayOneShot(painSound, 1.0f);
        t = 2.0f;

        if (hitPoints <= 0.0f) Die();
	}

    public void Medic(int medic)
    {
        hitPoints += medic;

        if (hitPoints > maxHitPoints)
        {
            float convertToScore = hitPoints - maxHitPoints;
            scoreManager.AddScore(System.Convert.ToInt32(convertToScore));
            hitPoints = maxHitPoints;
        }
		healthText.text = "+ " + hitPoints.ToString();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
		
		Color col = damageImage.color;
        col.a = 1.0f;
        damageImage.color = col;
		
		CanvasManager.instance.HideUI();

        Instantiate(deadReplacement, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void PlayerFallDamage(float dam)
    {
        ApplyDamage((int)dam);
        if (fallDamageSound) aSource.PlayOneShot(fallDamageSound, 1.0f);
    }

	IEnumerator Shake(float p)
    {
        float t = 1.0f;
        float shakePower;
        while (t > 0.0f)
        {
            t -= Time.deltaTime;
            shakePower = t / 50;
			
			camShake.localPosition = originalPos + Random.insideUnitSphere * shakePower * 35;
			yield return 0;
        }
    }
}