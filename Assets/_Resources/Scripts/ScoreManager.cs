using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
	public static ScoreManager instance = null;
	
    private float alphaHit;
    public AudioClip hitSound;
	public AudioClip levelUpSound;
    public AudioSource aSource;

    int pointsToNextRank = 500;
	public int currentScore = 0;
    public int lvl = 0;
	
	Image hitmarker;
	Text scoreText;
	Text levelText;

	void Awake()
	{
		instance = this;	
	}	

	void Start()
	{
		hitmarker = CanvasManager.instance.hitmarker;
		scoreText = CanvasManager.instance.scoreText;
		levelText = CanvasManager.instance.levelText;
	}	
	
    void Update()
    {
        if (alphaHit > 0f)
		{
            alphaHit -= Time.deltaTime;
			hitmarker.color = new Color(1, 1, 1, alphaHit);
		}	
    }

    public void DrawCrosshair()
    {
        alphaHit = 1.0f;
        aSource.PlayOneShot(hitSound, 0.2f);
    }

    public void AddScore(int val)
    {
        currentScore += val;
		scoreText.text = currentScore.ToString();
		if(currentScore > 0) CanvasManager.instance.scoreUI.SetActive(true);

        if (currentScore >= pointsToNextRank)
        {
            lvl++;
			levelText.text = lvl.ToString();
			if(lvl > 0) CanvasManager.instance.levelUI.SetActive(true);
            aSource.PlayOneShot(levelUpSound, 0.2f);
            pointsToNextRank += (lvl * 100);
        }
    }
}