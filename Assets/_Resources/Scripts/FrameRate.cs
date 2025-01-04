using UnityEngine;
using UnityEngine.UI;

public class FrameRate : MonoBehaviour 
{
	float upInterval = 1.0f;
	float accum = 0.0f; 
	int frames = 0; 
	float ti;
	int framerate;
	public Text framerateText;

	void Update () 
	{
		ti -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		frames ++;
	 
		if(ti <= 0.0f)
		{
			framerate = Mathf.FloorToInt(accum/frames);
			framerateText.text = framerate.ToString("F0");
			ti = upInterval;
			accum = 0.0f;
			frames = 0;
		}	
	}
}
