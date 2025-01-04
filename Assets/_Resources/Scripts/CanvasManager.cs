using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour {
	public static CanvasManager instance = null;
	
	public CanvasGroup playerCanvas;
	public CanvasGroup vehicleCanvas;
	
	[Header("Health")]
	public Image damageImage;
	public GameObject healthUI;
	public Text healthText;

	[Header("Ammo")]
	public GameObject ammoUI;
	public Text ammoText;
	public Text magText;
	
	[Header("Rockets")]
	public GameObject rocketsUI;
	public Text rocketsText;
	
	[Header("Level")]
	public GameObject levelUI;
	public Text levelText;
	
	[Header("Score")]
	public GameObject scoreUI;
	public Text scoreText;
	
	[Header("Timer")]
	public GameObject timerUI;
	public Text timerText;
	
	[Header("Target Result")]
	public GameObject resultUI;
	public Text targetScore;
	public Text targetKills;
	public Text targetHead;
	
	[Header("Minimap")]
	public GameObject minimapUI;
	
	[Header("Static Crosshair")]
	public GameObject staticCrossUI;
	public Image staticCross;
	
	[Header("Vehicle")]
	public Image vehicleDamageImage;
	public Text vehicleHealthText;
	public Text vehiclSpeedText;
	
	[Header("Dynamic Crosshair")]
	public GameObject dynamicCrossUI;
	public RectTransform crossUp;
	public RectTransform crossDown;
	public RectTransform crossLeft;
	public RectTransform crossRight;
	public CanvasGroup crossAlpha;
	float crossSize = 50f; 
	bool showCrosshair = false; 
	WeaponManager wepManager = null;
	float adjustSize = 10f;
	
	[Header("Other")]
	public Image hitmarker;
	public Image sniperScope;
	public Text note;
	public GameObject playerDead;
	public Image fadeImage;

	void Awake () {
		instance = this;
	}
	
	void Update(){
		if(showCrosshair){
			crossSize = wepManager.crosshairSize * adjustSize * (Screen.height / 600f);
			crossUp.localPosition = new Vector3(0f, crossSize, 0f);
			crossDown.localPosition = new Vector3(0f, -crossSize, 0f);
			crossRight.localPosition = new Vector3(crossSize, 0f, 0f);
			crossLeft.localPosition = new Vector3(-crossSize, 0f, 0f);
		}
	}
	
	public void SetWeapon(WeaponManager wep){
		wepManager = wep;
		showCrosshair = true;
		crossAlpha.alpha = 1f;
	}

	public void HideUI(){
		resultUI.SetActive(false);
		timerUI.SetActive(false);
		playerCanvas.alpha = 0f;
		crossAlpha.alpha = 0f;
		playerDead.SetActive(true);
		Fade(1);
	}
	
	public void ShowResult(string score, string kills, string head){
		targetScore.text = score;
		targetKills.text = kills;
		targetHead.text = head;
		StartCoroutine(ShowResultUI());
	}

	IEnumerator ShowResultUI(){
		resultUI.SetActive(true);
		yield return new WaitForSeconds(10.0f);
		resultUI.SetActive(false);
	}	
	
	public void PlayerInVehicle(bool inVehicle){
		
		if(inVehicle){
			playerCanvas.alpha = 0f;
			vehicleCanvas.alpha = 1f;
		}else{
			playerCanvas.alpha = 1f;
			vehicleCanvas.alpha = 0f;
		}	
	}

	public void UpdateAmmoUI(int ammo, int mags, int projectiles){
		ammoText.text = ammo.ToString();
		magText.text = mags.ToString();
		rocketsText.text = projectiles.ToString();
	}	
	
	public void UpdateBullets(int ammo){
		ammoText.text = ammo.ToString();
	}
	
	public void UpdateMags(int mags){
		magText.text = mags.ToString();
	}
	
	public void UpdateProjectileUI(int projectiles){
		rocketsText.text = projectiles.ToString();
	}	
	
	public void ShowProjectilesUI(bool show){
		rocketsUI.SetActive(show);
	}

	public void Fade (int fadeOption){
		if(fadeOption == 0){
			StartCoroutine(FadeOut());
		} else if(fadeOption == 1){
			StartCoroutine(FadeIn());
		}
	}
	
	IEnumerator FadeOut(){
		Color col = fadeImage.color;
		float alpha = 1f;
		while(alpha > 0f){
            col.a = alpha;
            fadeImage.color = col;
			alpha -= Time.deltaTime/2;	
			yield return null;
		}
	}	
	
	IEnumerator FadeIn(){
		yield return new WaitForSeconds(2);
		Color col = fadeImage.color;
		float alpha = 0f;
		while(alpha < 1.0f){
            col.a = alpha;
            fadeImage.color = col;
			alpha += Time.deltaTime/5;	
			yield return null;
		}
		SceneManager.LoadScene(0, LoadSceneMode.Single);
	}

	public void ShowCrosshair(int type){
		if(type == 0) //static
		{
			staticCrossUI.SetActive(true);
			dynamicCrossUI.SetActive(false);
		}
		else if(type == 1) //dynamic
		{
			staticCrossUI.SetActive(false);
			dynamicCrossUI.SetActive(true);
		}
		else //none
		{
			staticCrossUI.SetActive(false);
			dynamicCrossUI.SetActive(false);
		}
	}
}
