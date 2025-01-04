using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class GameManager : MonoBehaviour 
{
	public GameObject pauseMenu;
	public GameObject optionsMenu;
	public Transform spawn;
	public GameObject player;
	public Camera cam;
	public AudioListener aListener;
	public GameObject playerUI;
	BlurOptimized[] blur;
	GameObject playerClone;
	
	public Resolution[] resolutions;
	int res = 0;
	public Text resolutionsText;
	bool fullScreen;
	
	public Text qualityText;
	int qualityLevel = 0;
	string[] names;
	
	public Slider audioSlider;
	public Toggle fullscreenToggle;
	public AudioSource ambience;

	void Start()
	{
		SpawnPlayer();
		blur = FindObjectsOfType<BlurOptimized>();
		resolutions = Screen.resolutions;
		names = QualitySettings.names;
	}
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.M))
		{
			if(!playerClone) 
				return;
			
			if(pauseMenu && Time.timeScale == 1.0f) 
				pauseMenu.SetActive(true);
			
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			
			if (Time.timeScale == 1.0f)
			{
				Time.timeScale = 0.1f;
				Time.fixedDeltaTime = 0.02f * Time.timeScale;
				for(int i = 0; i < blur.Length; i++)
				{
					blur[i].enabled = true;
				}	
				UpdatePauseAudio();
			} 
		}
	}
	
	public void SpawnPlayer()
	{
		playerClone = Instantiate(player, spawn.position, spawn.rotation);
		playerClone.GetComponent<FPSController>().ambientSource = ambience;
		cam.enabled = aListener.enabled = false;
		CanvasManager.instance.Fade(0);
		playerUI.SetActive(true);
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}	
	
	public void CloseMenu()
	{
		if(!playerClone) 
			return;
		
		pauseMenu.SetActive(false);
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		
		Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
		
		for(int i = 0; i < blur.Length; i++)
		{
			blur[i].enabled = false;
		}
		UpdatePauseAudio();
	}
	
	void UpdatePauseAudio()
	{
		AudioSource[] aSources = FindObjectsOfType<AudioSource>();
		for (int i = 0; i < aSources.Length; i++)
		{
			aSources[i].pitch = Time.timeScale;
		}	
	}
	
	public void Options()
	{
		pauseMenu.SetActive(false);
		optionsMenu.SetActive(true);

		fullScreen = Screen.fullScreen;
		fullscreenToggle.isOn = fullScreen;
		qualityLevel = QualitySettings.GetQualityLevel();
		qualityText.text = names[qualityLevel];
		audioSlider.value = AudioListener.volume;
		
		#if UNITY_EDITOR
			resolutionsText.text = Screen.width + "x" + Screen.height;
		#else	
			for(int i = 0; i < resolutions.Length; i++){
				if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height){
					resolutionsText.text = resolutions[i].width + "x" + resolutions[i].height;
					res = i;
					break;
				}	
			}
		#endif		
	}
	
	public void Back()
	{
		pauseMenu.SetActive(true);
		optionsMenu.SetActive(false);
		fullScreen = fullscreenToggle.isOn;
		Screen.SetResolution (resolutions[res].width, resolutions[res].height, fullScreen);
		QualitySettings.SetQualityLevel (qualityLevel, false); 
		AudioListener.volume = audioSlider.value;
	}	
	
	public void ExitGame()
	{
		Application.Quit();
	}	
	
	public void ResolutionDecrease()
	{
		if(res < resolutions.Length)
		{ 
			res--;	
			if(res < 0) res = resolutions.Length - 1;
		}
		resolutionsText.text = resolutions[res].width + "x" + resolutions[res].height;
	}			

	public void ResolutionIncrease()
	{	
		if(res < resolutions.Length)
		{ 
			res++;
			if(res > (resolutions.Length - 1)) res = 0;	
		}
		resolutionsText.text = resolutions[res].width + "x" + resolutions[res].height;	
	}

	public void QualityDecrease()
	{
		if(qualityLevel < names.Length)
		{ 
			qualityLevel--;	
			if(qualityLevel < 0) qualityLevel = names.Length - 1;
		}
		qualityText.text = names[qualityLevel];
	}			

	public void QualityIncrease()
	{	
		if(qualityLevel < names.Length)
		{ 
			qualityLevel++;
			if(qualityLevel > (names.Length - 1)) qualityLevel = 0;	
		}
		qualityText.text = names[qualityLevel];	
	}
}
