using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Image[] settingImages;
    [SerializeField] private Image[] secondarySettingImages;
    [SerializeField] private Image[] tertiarySettingImages;
    [SerializeField] private Image[] quaternarySettingImages;
    [SerializeField] private Image[] quinarySettingImages;

    [SerializeField] private Button[] settingButtons;
    [SerializeField] private Button[] secondarySettingButtons;
    [SerializeField] private Button[] tertiarySettingButtons;
    [SerializeField] private Button[] quaternarySettingButtons;
    [SerializeField] private Button[] quinarySettingButtons;

    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite unselectedSprite;

    private int currentSelectedIndex = -1;
    private int currentSecondarySelectedIndex = -1;
    private int currentTertiarySelectedIndex = -1;
    private int currentQuaternarySelectedIndex = -1;
    private int currentQuinarySelectedIndex = -1;

    void Start()
    {
        // Initialize primary settings
        for (int i = 0; i < settingImages.Length; i++)
        {
            settingImages[i].sprite = unselectedSprite;
            int index = i;
            settingButtons[i].onClick.AddListener(() => SelectImage(index));
        }

        // Initialize secondary settings
        for (int i = 0; i < secondarySettingImages.Length; i++)
        {
            secondarySettingImages[i].sprite = unselectedSprite;
            int index = i;
            secondarySettingButtons[i].onClick.AddListener(() => SelectSecondaryImage(index));
        }

        // Initialize tertiary settings
        for (int i = 0; i < tertiarySettingImages.Length; i++)
        {
            tertiarySettingImages[i].sprite = unselectedSprite;
            int index = i;
            tertiarySettingButtons[i].onClick.AddListener(() => SelectTertiaryImage(index));
        }

        // Initialize quaternary settings
        for (int i = 0; i < quaternarySettingImages.Length; i++)
        {
            quaternarySettingImages[i].sprite = unselectedSprite;
            int index = i;
            quaternarySettingButtons[i].onClick.AddListener(() => SelectQuaternaryImage(index));
        }

        // Initialize quinary settings with alpha 0
        for (int i = 0; i < quinarySettingImages.Length; i++)
        {
            Color imageColor = quinarySettingImages[i].color;
            imageColor.a = 0f;
            quinarySettingImages[i].color = imageColor;
            int index = i;
            quinarySettingButtons[i].onClick.AddListener(() => SelectQuinaryImage(index));
        }

        // Default selections
        SelectImage(0);
        SelectSecondaryImage(0);
        SelectTertiaryImage(0);
        SelectQuaternaryImage(0);
        SelectQuinaryImage(0);
    }

    public void SelectImage(int index)
    {
        if (currentSelectedIndex >= 0)
        {
            settingImages[currentSelectedIndex].sprite = unselectedSprite;
        }
        currentSelectedIndex = index;
        settingImages[currentSelectedIndex].sprite = selectedSprite;
    }

    public void SelectSecondaryImage(int index)
    {
        if (currentSecondarySelectedIndex >= 0)
        {
            secondarySettingImages[currentSecondarySelectedIndex].sprite = unselectedSprite;
        }
        currentSecondarySelectedIndex = index;
        secondarySettingImages[currentSecondarySelectedIndex].sprite = selectedSprite;
    }

    public void SelectTertiaryImage(int index)
    {
        if (currentTertiarySelectedIndex >= 0)
        {
            tertiarySettingImages[currentTertiarySelectedIndex].sprite = unselectedSprite;
        }
        currentTertiarySelectedIndex = index;
        tertiarySettingImages[currentTertiarySelectedIndex].sprite = selectedSprite;
    }

    public void SelectQuaternaryImage(int index)
    {
        if (currentQuaternarySelectedIndex >= 0)
        {
            quaternarySettingImages[currentQuaternarySelectedIndex].sprite = unselectedSprite;
        }
        currentQuaternarySelectedIndex = index;
        quaternarySettingImages[currentQuaternarySelectedIndex].sprite = selectedSprite;
    }

    public void SelectQuinaryImage(int index)
    {
        if (currentQuinarySelectedIndex >= 0)
        {
            Color imageColor = quinarySettingImages[currentQuinarySelectedIndex].color;
            imageColor.a = 0f;
            quinarySettingImages[currentQuinarySelectedIndex].color = imageColor;
        }
        currentQuinarySelectedIndex = index;
        Color newImageColor = quinarySettingImages[currentQuinarySelectedIndex].color;
        newImageColor.a = 1f;
        quinarySettingImages[currentQuinarySelectedIndex].color = newImageColor;
        quinarySettingImages[currentQuinarySelectedIndex].sprite = selectedSprite;
    }
}