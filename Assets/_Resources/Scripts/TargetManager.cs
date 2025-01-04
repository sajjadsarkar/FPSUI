using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TargetManager : MonoBehaviour 
{
    public Target[] allTargets;
    public float hitpoints = 100f;

    private float timer = 0.0f;
    public float duration = 30.0f;
    private int trainingScore = 0;
    private int kills = 0;
    private int headshots = 0;
    [HideInInspector] public int state = 0;

    public AudioSource aSource;
    public AudioClip countdownSound;
	Text timerText;

    void Start()
    {
        state = 0;
		timerText = CanvasManager.instance.timerText;

        for (int i = 0; i < allTargets.Length; i++)
        {
            allTargets[i].baseHitPoints = hitpoints;
            allTargets[i].trainingMode = false;
            StartCoroutine(allTargets[i].TargetUp());
        }
    }

    public void NextTarget()
    {
        StartCoroutine(allTargets[Random.Range(0, allTargets.Length)].TargetUp());
    }

    void StartTraining()
    {
        for (int i = 0; i < allTargets.Length; i++)
        {
            allTargets[i].baseHitPoints = hitpoints;
            allTargets[i].trainingMode = true;
            StartCoroutine(allTargets[i].TargetDown());
        }

        trainingScore = 0;
        headshots = 0;
        kills = 0;
        timer = duration;
        state = 1;
        StartCoroutine(Ready());
    }

    IEnumerator Ready()
    {
        aSource.PlayOneShot(countdownSound, 0.5f);
        yield return new WaitForSeconds(6.0f);
        state = 2;
        NextTarget();
    }

    void Update()
    {
        if (state == 2)
        {
            timer -= Time.deltaTime;

            if (timer <= 0.0f)
            {
               StartCoroutine(TrainingEnds());
            }
        }
		
		if (state == 1 || state == 2)
		{	 
			timerText.text = FormatSeconds(timer);
		} 
    }

    public void SetScore(int s, bool hs)
    {
        trainingScore += s;
        kills++;
        if (hs) headshots++;
    }

    IEnumerator TrainingEnds()
    {
        state = 3;
		CanvasManager.instance.timerUI.SetActive(false);
		CanvasManager.instance.ShowResult("<color=#88FF6AFF>SCORE :  </color>" + trainingScore, "<color=#88FF6AFF>KILLS :  </color>" + kills, "<color=#88FF6AFF>HEADSHOTS :  </color>" + headshots);	
		
        yield return new WaitForSeconds(10.0f);
        state = 0;
        for (int i = 0; i < allTargets.Length; i++)
        {
            allTargets[i].baseHitPoints = hitpoints;
            allTargets[i].trainingMode = false;
            StartCoroutine(allTargets[i].TargetUp());
        }
    }

    string FormatSeconds(float elapsed)
    {
        int d = (int)(elapsed * 100.0f);
        int minutes = d / (60 * 100);
        int seconds = (d % (60 * 100)) / 100;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
	
    void Action()
    {
        if (state == 0)
		{
			StartTraining();
			CanvasManager.instance.timerUI.SetActive(true);	
		}	
    }
}