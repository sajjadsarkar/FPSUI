using UnityEngine;
using UnityEngine.UI;

public class VehicleDamage : MonoBehaviour
{
    public GameObject[] unparentWheels;
    public int hitPoints = 250;
	float t = 0f;
    public GameObject explosion;
    public GameObject body;
    public GameObject trigger;
    public VehicleScript vechicleScript;
	public ParticleSystem particles;
	Image vehicleDamageImage;
	Text vehicleHealthText;
	public Rigidbody rb;

    void Start()
    {
		vehicleHealthText = CanvasManager.instance.vehicleHealthText;
		vehicleDamageImage = CanvasManager.instance.vehicleDamageImage;
	}
	
	void Update()
    {
        if (t > 0.0f)
        {
            t -= Time.deltaTime;
			
			if(hitPoints > 0) 
			{
				Color col = vehicleDamageImage.color;
				col.a = t;
				vehicleDamageImage.color = col;
			}	
        }	
    }
	
	public void ApplyDamage(int damage)
    {
       if (hitPoints <= 0) return;

        hitPoints -= damage;
		if(vechicleScript.inVehicle)
		{
			t = 2.0f;
			UpdateHealthUI();
		}	
        if (hitPoints <= 0) Detonate();
    }
	
	public void UpdateHealthUI()
	{
		vehicleHealthText.text = "+ " + hitPoints.ToString("F0");
	}	
	
    void Detonate()
    {
		AudioSource[] aSources = GetComponentsInChildren<AudioSource>();
        foreach (AudioSource source in aSources) {
            source.enabled = false;
        }

        Component[] coms = GetComponentsInChildren<MonoBehaviour>();
        foreach (var b in coms)
        {
            MonoBehaviour p = b as MonoBehaviour;
            if (p) p.enabled = false;
        }
        trigger.SetActive(false);
		rb.AddForce(Vector3.up * Random.Range(5000, 30000), ForceMode.Impulse);
		rb.AddTorque(transform.up * Random.Range(-700000, 700000));
		rb.AddTorque(transform.forward * Random.Range(-700000, 700000));
        for (int i = 0; i < unparentWheels.Length; i++)
        {
            unparentWheels[i].transform.parent = null;
            unparentWheels[i].AddComponent<MeshCollider>();
            unparentWheels[i].GetComponent<MeshCollider>().convex = true;
            unparentWheels[i].AddComponent<Rigidbody>();
            unparentWheels[i].GetComponent<Rigidbody>().mass = 12;
            unparentWheels[i].transform.position = new Vector3(unparentWheels[i].transform.position.x, unparentWheels[i].transform.position.y + 1, unparentWheels[i].transform.position.z);
        }
		if(particles) particles.Stop(true);
        if(explosion) Instantiate(explosion, body.transform.position, body.transform.rotation);
        transform.DetachChildren();
        if(vechicleScript.inVehicle) vechicleScript.GetOut();
    }
}